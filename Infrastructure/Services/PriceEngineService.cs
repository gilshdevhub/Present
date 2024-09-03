using Core.Entities.PriceEngine;
using Core.Enums;
using Core.Interfaces;
using Dapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Services;

public class PriceEngineService : IPriceEngineService
{
    private readonly DapperContext _context;
    private readonly ICacheService _cacheService;
    private readonly RailDbContext _railDbContext;


    public PriceEngineService(DapperContext context, ICacheService cacheService, RailDbContext railDbContext)
    {
        _context = context;
        _cacheService = cacheService;
        _railDbContext = railDbContext;
    }




    public async Task<GetAllPriceResultObject> GetAllPriceResultAsync()
    {
        var key = "GetAllPriceResult";
        GetAllPriceResultObject cacheObj = await _cacheService.GetAsync<GetAllPriceResultObject>(key).ConfigureAwait(false);

        if (cacheObj == null || cacheObj.AllSourceToDestination == null || cacheObj.AllProfileDiscount == null)
        {
            cacheObj = await GetAllPrice().ConfigureAwait(false);
            await _cacheService.SetAsync<GetAllPriceResultObject>(key, cacheObj).ConfigureAwait(false);
        }

        return cacheObj;
    }

    public async Task<GetAllPriceResultObjectWithNotes> GetAllPriceResultWithNotesAsync()
    {
        GetAllPriceResultObjectWithNotes cacheObj = await _cacheService.GetAsync<GetAllPriceResultObjectWithNotes>(CacheKeys.GetAllPriceResultWithNotes).ConfigureAwait(false);

        if (cacheObj == null || cacheObj.AllSourceToDestination == null || cacheObj.AllProfileDiscount == null || cacheObj.PriceNotes == null)
        {
            cacheObj = await GetAllPriceWithNotesWithNotes().ConfigureAwait(false);
            await _cacheService.SetAsync<GetAllPriceResultObjectWithNotes>(CacheKeys.GetAllPriceResultWithNotes, cacheObj).ConfigureAwait(false);
        }

        return cacheObj;
    }


    public async Task<IEnumerable<priceDetails>> GetPriceResultAsync(getPriceRequest dto)
    {
        IEnumerable<priceDetails> priceDetails = await GetPrice(dto).ConfigureAwait(false);
        
        return priceDetails;
    }

    public async Task<IEnumerable<profileDetails>> getProfilesResultAsync(getProfiles dto)
    {
        List<Profile_Filtering> profile_Filterings = (await GetAllProfileFilteringAsync()).Where(x => x.Request_Id == dto.RequestId).ToList();
        var key = "getProfilesResult";
        IEnumerable<profileDetails> profileDetails = (await _cacheService.GetAsync<IEnumerable<profileDetails>>(key).ConfigureAwait(false));

        if (profileDetails == null || profileDetails.Count() <= 0)
        {
            profileDetails = await getProfiles(dto).ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<profileDetails>>(key, profileDetails).ConfigureAwait(false);
        }

        return profileDetails.Where(x => profile_Filterings.Any(y => y.Profile_Id == x.Profile_Id)); ;
    }


    public async Task<IEnumerable<PriceNotesResponseDto>> GetAllPriceNotesAsync()
    {

        return await _railDbContext.PriceNotes.Join(_railDbContext.Profiles,
                                                           x => x.Profile_Id,



               y => y.Profile_Id,

                                                           (x, y) =>
                                                           new PriceNotesResponseDto
                                                           {
                                                               DayNote = x.DayNote,
                                                               Profile_Id = x.Profile_Id,
                                                               Heb_Profile_Desc = y.Heb_Profile_Desc,
                                                               PriceNoteEn = x.PriceNoteEn,
                                                               Id = x.Id,
                                                               MonthNote = x.MonthNote,
                                                               PriceNoteAr = x.PriceNoteAr,
                                                               PriceNoteHe = x.PriceNoteHe,
                                                               PriceNoteRu = x.PriceNoteRu,
                                                               SingleNote = x.SingleNote
                                                           })
            .ToArrayAsync().ConfigureAwait(false);
    }
    public async Task<IEnumerable<Profiles>> GetAllProfiles()
    {
        return await _railDbContext.Profiles.ToArrayAsync();
    }

    public async Task<IEnumerable<Profile_Filtering>> GetAllProfileFilteringAsync()
    {
        IEnumerable<Profile_Filtering> profile_Filterings = (await _cacheService.GetAsync<IEnumerable<Profile_Filtering>>(CacheKeys.Profile_Filtering).ConfigureAwait(false));
        if (profile_Filterings == null || profile_Filterings.Count() <= 0)
        {
            profile_Filterings = await _railDbContext.Profile_Filtering.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.Profile_Filtering, profile_Filterings).ConfigureAwait(false);
        }

        return profile_Filterings;
    }

    private async Task<GetAllPriceResultObject> GetAllPrice()

    {
        const string spname = "[dbo].[sp_TEngine_getAllPrice]";
        using var connection = _context.CreateConnection();
        connection.Open();
        var reader = await connection.QueryMultipleAsync(spname, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        var setA = reader.Read<SourceToDestinationDTO>().ToList();
        var setB = reader.Read<Profile_Discount_ContractDTO>().ToList();
        return new GetAllPriceResultObject { AllSourceToDestination = setA, AllProfileDiscount = setB };
    }

    private async Task<GetAllPriceResultObjectWithNotes> GetAllPriceWithNotesWithNotes()
    {
        try
        {
            const string spname = "[dbo].[sp_TEngine_getAllPrice]";
            using var connection = _context.CreateConnection();

            connection.Open();

            var reader = await connection.QueryMultipleAsync(spname, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
            List<SourceToDestinationDTO> setA = reader.Read<SourceToDestinationDTO>().ToList();
            List<Profile_Discount_ContractDTO> setB = reader.Read<Profile_Discount_ContractDTO>().ToList();
            List<PriceNotes> PriceNotes = await _railDbContext.PriceNotes.ToListAsync();

            GetAllPriceResultObjectWithNotes res = new GetAllPriceResultObjectWithNotes { AllSourceToDestination = setA, AllProfileDiscount = setB, PriceNotes = PriceNotes };
            return res;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    private async Task<IEnumerable<priceDetails>> GetPrice(getPriceRequest dto)

    {
        const string spname = "[dbo].[sp_TEngine_getPrice]";
        using var connection = _context.CreateConnection();
        connection.Open();
        return await connection.QueryAsync<priceDetails>(spname,
            new { dto.RequestId, dto.Profile_ID, dto.From_Station_Code_ISR, dto.To_Station_Code_ISR }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

    }

    private async Task<IEnumerable<profileDetails>> getProfiles(getProfiles dto)

    {
        const string spname = "[dbo].[sp_TEngine_getProfiles]";
        using var connection = _context.CreateConnection();
        connection.Open();
        return await connection.QueryAsync<profileDetails>(spname,
            new { dto.RequestId }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

    }


    private Task<int> CompleteAsync()
    {
        return _railDbContext.SaveChangesAsync();
    }
    private async Task<PriceNotes> GetItemByIdAsync(int id)
    {
        PriceNotes priceNotes = await _railDbContext.PriceNotes.SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
        return priceNotes;
    }


    #region AddDeleteUpdate
    public async Task<PriceNotes> AddPriceNoteAsync(PriceNotes item)
    {
        var test = _railDbContext.PriceNotes.Where(x => x.Profile_Id == item.Profile_Id);
        if (test.Count() > 0)
            return null;
        var entity = _railDbContext.PriceNotes.Add(item);
        PriceNotes priceNote = entity.Entity;

        bool success = await CompleteAsync() > 0;

        if (success)
        {
            await UpdateCache().ConfigureAwait(false);

            return priceNote;
        }
        else
        {
            return null;
        }
    }

    private async Task UpdateCache()
    {
        try
        {
                     await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.GetAllPriceResultWithNotes);
            var cacheObj = await GetAllPriceWithNotesWithNotes().ConfigureAwait(false);
            await _cacheService.SetAsync<GetAllPriceResultObjectWithNotes>(Core.Enums.CacheKeys.GetAllPriceResultWithNotes, cacheObj).ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.PriceNotes);
            IEnumerable<PriceNotes> priceNotes = await _railDbContext.PriceNotes.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<PriceNotes>>(Core.Enums.CacheKeys.PriceNotes, priceNotes).ConfigureAwait(false);
        }
        catch (Exception ex)
        {

        }
    }

    public async Task<bool> DeletePriceNoteAsync(int id)
    {
        PriceNotes priceNote = await GetItemByIdAsync(id);
        bool success = false;
        if (priceNote != null)
        {
            _ = _railDbContext.PriceNotes.Remove(priceNote);
            success = await _railDbContext.SaveChangesAsync() > 0;
            if (success)
            {
                await UpdateCache().ConfigureAwait(false);
            }
        }

        return success;
    }
    public async Task<bool> UpdatePriceNoteAsync(PriceNotes item)
    {
        _ = _railDbContext.PriceNotes.Attach(item);
        _railDbContext.Entry(item).State = EntityState.Modified;
        bool success = await CompleteAsync() > 0;
        if (success)
        {
            await UpdateCache().ConfigureAwait(false);
        }
        return success;
    }
    #endregion AddDeleteUpdate
}
