using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class StationGateActivityHoursService : IStationGateActivityHoursService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public StationGateActivityHoursService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }
    public async Task<IEnumerable<StationGateActivityHoursDto>> GetStationGateActivityHoursAsync()
    {
        return await GetStationGateActivityHoursNoCache().ConfigureAwait(false);
    }

    public async Task<Dictionary<int, List<StationGateActivityHoursDto>>> GetHoursByStationAndTemplateTypeAsync(int stationId, int templateTypeId, IEnumerable<StationGateDto> stationGates)
    {
        IEnumerable<StationGateActivityHoursDto> stationGateActivityHours = await GetStationGateActivityHoursNoCache().ConfigureAwait(false);
        IEnumerable<int> gatesIds = stationGates.Where(x => x.StationId == stationId).Select(x => x.StationGateId).ToArray();


        Dictionary<int, List<StationGateActivityHoursDto>> result = stationGateActivityHours
                                    .Where(x => gatesIds.Contains(x.StationGateId) && x.TemplateTypeId == templateTypeId)
                                    .GroupBy(x => x.StationGateId)
                                    .ToDictionary(g => g.Key, g => g.ToList()); ;
        return result;


    }
    public async Task<IEnumerable<StationGateActivityHoursDto>> GetStationGateActivityHoursByGateIdTemplateIdAsync(int templateTypeId, int stationGateId)
    {
        var stationGateActivityHours = (await GetStationGateActivityHoursNoCache().ConfigureAwait(false)).Where(x => x.StationGateId == stationGateId && x.TemplateTypeId == templateTypeId).ToArray();
               return stationGateActivityHours;
    }

    public async Task<List<string>> PreAddStationGateActivityHour(List<int> StationGatesIds)
    {
        IEnumerable<StationGateActivityHours> stationGateActivityHours = await _context.StationGateActivityHours.ToArrayAsync();
        List<int> GateIds = stationGateActivityHours.Where(x => StationGatesIds.Contains(x.StationGateId)).Select(x => x.StationGateId).ToList();
        IEnumerable<StationGate> stationGate = await _context.StationGate.ToListAsync();
        IEnumerable<Translation> translations = await _context.Translations.ToListAsync();
        return stationGate.Where(x => GateIds.Contains(x.StationGateId)).Select(x => translations.Where(y => y.Key == x.GateNameTranslationKey).Select(y => y.Hebrew).FirstOrDefault()).ToList();
    }

    private async Task<IEnumerable<StationGateActivityHoursDto>> GetStationGateActivityHoursNoCache()
    {
        IEnumerable<StationGateActivityHoursDto> stationGateActivityHours = await _context.StationGateActivityHours
                                                                                                                                                                                 .Join(_context.StationGateActivityHoursLines,
                                                                x => x.StationHoursId,
                                                                y => y.StationHoursId,
                                                                (x, y) =>


                                                                new StationGateActivityHoursDto
                                                                {
                                                                    StationHoursId = x.StationHoursId,
                                                                    ActivityDaysNumbers = y.ActivityDaysNumbers,
                                                                    EndHour = y.EndHour,
                                                                    StartHour = y.StartHour,
                                                                    StationGateId = x.StationGateId,
                                                                    StationHoursLineId = y.Id,
                                                                    TemplateTypeId = x.TemplateTypeId,
                                                                    ActivityHoursReplaceTextKey = y.ActivityHoursReplaceTextKey,
                                                                    EndHourPostfixTextKey = y.EndHourPostfixTextKey,
                                                                    EndHourPrefixTextKey = y.EndHourPostfixTextKey,
                                                                    EndHourReplaceTextKey = y.EndHourReplaceTextKey,
                                                                    StartHourPrefixTextKey = y.StartHourPrefixTextKey,
                                                                    StartHourReplaceTextKey = y.StartHourReplaceTextKey,
                                                                    IsClosed = x.IsClosed,
                                                                    ClosedUntill = x.ClosedUntill
                                                                })

                                                             .ToArrayAsync().ConfigureAwait(false);

        return stationGateActivityHours;
    }

    public async Task<int> StationHoursIdByGateId(int gateId, int templateTypeid)
    {
        IEnumerable<StationGateActivityHoursDto> stationGateActivityHours = await GetStationGateActivityHoursNoCache().ConfigureAwait(false);
        return stationGateActivityHours.Where(x => x.StationGateId == gateId && x.TemplateTypeId == templateTypeid).Select(x => x.StationHoursId).FirstOrDefault();
    }

    #region AddDeleteUpdate
    public async Task<bool> AddStationGateActivityHourAsync(StationGateActivityHours StationGateActivityHours, IEnumerable<StationGateActivityHoursLines> StationGateActivityHoursLines)
    {
        EntityEntry<StationGateActivityHours> entity = await _context.StationGateActivityHours.AddAsync(StationGateActivityHours).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            _ = entity.Entity;

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationGateActivityHours);
        }

        foreach (var item in StationGateActivityHoursLines)
        {
            item.StationHoursId = entity.Entity.StationHoursId;
            _ = await _context.StationGateActivityHoursLines.AddAsync(item).ConfigureAwait(false);
        }
        _ = await _context.SaveChangesAsync().ConfigureAwait(false);
        IEnumerable<StationGateActivityHoursDto> stationGateActivityHours = await GetStationGateActivityHoursNoCache();
        await _cacheService.SetAsync<IEnumerable<StationGateActivityHoursDto>>(CacheKeys.StationGateActivityHours, stationGateActivityHours).ConfigureAwait(false);
        return true;
    }
    public async Task<bool> DeleteGateActivityHourAsync(int Id)
    {
        StationGateActivityHours template = await _context.StationGateActivityHours.SingleOrDefaultAsync(p => p.StationHoursId == Id).ConfigureAwait(false);
        bool success = false;
        if (template != null)
        {
            try
            {
                _ = _context.StationGateActivityHours.Remove(template);
                success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
                if (success)
                {
                    await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationGateActivityHours);
                    IEnumerable<StationGateActivityHoursDto> stationGateActivityHours = await GetStationGateActivityHoursNoCache();
                    await _cacheService.SetAsync<IEnumerable<StationGateActivityHoursDto>>(CacheKeys.StationGateActivityHours, stationGateActivityHours).ConfigureAwait(false);
                    return success;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        return false;
    }
    public async Task<bool> UpdateStationGateActivityHourAsync(StationGateActivityHours StationGateActivityHours, IEnumerable<StationGateActivityHoursLines> StationGateActivityHoursLines)
    {
        EntityEntry<StationGateActivityHours> entity = _context.StationGateActivityHours.Update(StationGateActivityHours);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            _ = entity.Entity;

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationGateActivityHours);
        }

        foreach (var item in StationGateActivityHoursLines)
        {
            _ = _context.StationGateActivityHoursLines.Update(item);
        }
        bool result = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        IEnumerable<StationGateActivityHoursDto> stationGateActivityHours = await GetStationGateActivityHoursNoCache();
        await _cacheService.SetAsync<IEnumerable<StationGateActivityHoursDto>>(CacheKeys.StationGateActivityHours, stationGateActivityHours).ConfigureAwait(false);
        return result;
    }
    #endregion AddDeleteUpdate
}
