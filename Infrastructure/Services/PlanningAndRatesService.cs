using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class PlanningAndRatesService : IPlanningAndRatesService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ITendersCommonService _tendersCommonService;
    public PlanningAndRatesService(RailDbContext context, ICacheService cacheService, ITendersCommonService tendersCommonService)
    {
        _context = context;
        _cacheService = cacheService;
        _tendersCommonService = tendersCommonService;
    }

   public async Task<IEnumerable<PlanningAndRatesDto>> GetPlanningAndRatesAsync(int categoryId)
    {
        IEnumerable<PlanningAndRatesDto> allPlanningAndRates = (await GetNoCache().ConfigureAwait(false)).Where(x => x.Category == categoryId); 

        return allPlanningAndRates;
    }

    public async Task<IEnumerable<PlanningAndRatesDto>> GetPlanningAndRatesAsync()
    {
        IEnumerable<PlanningAndRatesDto> planningAndRatess = (await GetPlanningAndRatesContentAsync().ConfigureAwait(false));

        return planningAndRatess;
    }

    public async Task<PlanningAndRatesDto> GetPlanningAndRatesByIdAsync(Guid Id)
    {
        PlanningAndRatesDto planningAndRatess = (await GetPlanningAndRatesContentAsync().ConfigureAwait(false)).SingleOrDefault(x => x.Id == Id);

        return planningAndRatess;
    }


    private async Task<IEnumerable<PlanningAndRatesDto>> GetPlanningAndRatesContentAsync()
    {
        _ = await _tendersCommonService.GetTenderDocumentsContentAsync().ConfigureAwait(false);
        IEnumerable<PlanningAndRatesDto> allPlanningAndRates = await _cacheService.GetAsync<IEnumerable<PlanningAndRatesDto>>(CacheKeys.PlanningAndRates).ConfigureAwait(false);

        if (allPlanningAndRates == null || allPlanningAndRates.Count() <= 0)
        {
            allPlanningAndRates = await UpdateCachePlanningAndRatesAsync();
        }

        return allPlanningAndRates;
    }

    public async Task<IEnumerable<PlanningAndRatesDto>> UpdateCachePlanningAndRatesAsync()
    {
        IEnumerable<PlanningAndRatesDto> allPlanningAndRates = await GetNoCache().ConfigureAwait(false);
        await _cacheService.SetAsync(CacheKeys.PlanningAndRates, allPlanningAndRates).ConfigureAwait(false);

        return allPlanningAndRates;
    }
    public async Task<IEnumerable<PlanningAndRatesDto>> GetNoCache()
    {
        IEnumerable <PlanningAndRatesDto> allPlanningAndRates = await _context.PlanningAndRates.Select(x => new PlanningAndRatesDto
        {
            Id = x.Id,
            Subject = x.Subject,
            UpdatingUser = x.UpdatingUser,
            Language = x.Language,
            PlanningAreas = x.PlanningAreas,
            Filed = x.Domain,
            Type = x.TypeOfTender,
            Category = x.Page,
            UpdateDate = x.UpdateDate,
            Documentation = _context.TenderDocuments
                .Where(y => y.PlanningAndRatesId == x.Id)
                .Select(x => new TenderDocumentsDto
                {
                    DocDisplay = x.DocDisplay,
                    DocName = x.DocName,
                    DocType = x.DocType,
                    Id = x.Id
                }).ToArray(),
            SerialNumber = x.SerialNumber
        })
            .ToArrayAsync().ConfigureAwait(false);
     
        return allPlanningAndRates;
    }

    #region AddDeleteUpdate
    public async Task<PlanningAndRates> AddPlanningAndRatesAsync(PlanningAndRates planningAndRates)
    {
        PlanningAndRates planningAndRatess = await _context.PlanningAndRates
            .Where(i => i.Id == planningAndRates.Id).SingleOrDefaultAsync();

        if (planningAndRatess != null)
        {
            return null;
        }
        EntityEntry<PlanningAndRates> entity = _context.PlanningAndRates.Add(planningAndRates);
        PlanningAndRates newExemptionNotice = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.PlanningAndRates);
            _ = await UpdateCachePlanningAndRatesAsync();
            return newExemptionNotice;
        }
        return newExemptionNotice;
    }

    public async Task<bool> DeletePlanningAndRatesAsync(Guid Id)
    {
        PlanningAndRates item = await _context.PlanningAndRates.SingleOrDefaultAsync(p => p.Id == Id);
        if (item != null)
        {
            IEnumerable< TenderDocuments> docs = _context.TenderDocuments.Where(x=>x.PlanningAndRatesId== Id);
            if(docs!=null)
            {
                _context.TenderDocuments.RemoveRange(docs.First(), docs.Last());
            }
        }
        _ = _context.PlanningAndRates.Remove(item);
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.PlanningAndRates);
            _ = await UpdateCachePlanningAndRatesAsync();
        }
        return success;
    }
    public async Task<PlanningAndRates> UpdatePlanningAndRatesAsync(PlanningAndRates planningAndRatessToUpdate)
    {
        PlanningAndRates planningAndRatess = await _context.PlanningAndRates.SingleOrDefaultAsync(p => p.Id == planningAndRatessToUpdate.Id);

        if (planningAndRatess != null)
        {
            _ = _context.PlanningAndRates.Attach(planningAndRatess);
            _ = _context.PlanningAndRates.Remove(planningAndRatess);

            EntityEntry<PlanningAndRates> entity = _context.PlanningAndRates.Attach(planningAndRatessToUpdate);

            _context.Entry(planningAndRatessToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.PlanningAndRates);
                planningAndRatess = entity.Entity;
                _ = await UpdateCachePlanningAndRatesAsync();
            }
        }

        return planningAndRatess;
    }
    #endregion AddDeleteUpdate

}
