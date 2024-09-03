using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class ExemptionNoticesService : IExemptionNotices
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ITendersCommonService _tendersCommonService;
    public ExemptionNoticesService(RailDbContext context, ICacheService cacheService, ITendersCommonService tendersCommonService)
    {
        _context = context;
        _cacheService = cacheService;
        _tendersCommonService = tendersCommonService;
    }

  
    public async Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesAsync(int categoryId)
    {
        IEnumerable<ExemptionNoticesDto> exemptionNotices = (await GetExemptionNoticesNoCache().ConfigureAwait(false)).Where(x => x.Category == categoryId);

        return exemptionNotices;
    }
    public async Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesAsync()
    {
        IEnumerable<ExemptionNoticesDto> exemptionNotices = (await GetExemptionNoticesContentAsync().ConfigureAwait(false));

        return exemptionNotices;
    }

    public async Task<ExemptionNoticesDto> GetExemptionNoticesByIdAsync(Guid Id)
    {
        ExemptionNoticesDto exemptionNotices = (await GetExemptionNoticesContentAsync().ConfigureAwait(false)).SingleOrDefault(x => x.Id == Id);

        return exemptionNotices;
    }

   
    private async Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesContentAsync()
    {
        IEnumerable<ExemptionNoticesDto> allExemptionNotices = await _cacheService.GetAsync<IEnumerable<ExemptionNoticesDto>>(CacheKeys.ExemptionNotices).ConfigureAwait(false);

        if (allExemptionNotices == null)
        {
            allExemptionNotices = await UpdateCacheExemptionNoticesAsync();
        }

        return allExemptionNotices;
    }

    public async Task<IEnumerable<ExemptionNoticesDto>> UpdateCacheExemptionNoticesAsync()
    {
        IEnumerable<ExemptionNoticesDto> allExemptionNotices = await GetExemptionNoticesNoCache().ConfigureAwait(false);

        await _cacheService.SetAsync(CacheKeys.ExemptionNotices, allExemptionNotices).ConfigureAwait(false);


        return allExemptionNotices;
    }

    public async Task<IEnumerable<ExemptionNoticesDto>> GetExemptionNoticesNoCache()
    {
        IEnumerable<ExemptionNoticesDto> allExemptionNotices = await _context.ExemptionNotices.Select(x => new ExemptionNoticesDto
        {
            Id = x.Id,
            PublishDate = x.PublishDate,
            SupplierName = x.SupplierName,
            ReferentName = x.ReferentName,
            ReferentMail = x.ReferentMail,
            Subject = x.Subject,
            UpdatingUser = x.UpdatingUser,
            Language = x.Language,
            Filed = x.Domain,
            Type = x.TypeOfTender,
            Category = x.Page,
            UpdateDate = x.UpdateDate,
            Documentation = _context.TenderDocuments
                .Where(y => y.ExemptionNoticesId == x.Id)
                .Select(x => new TenderDocumentsDto
                {
                    DocDisplay = x.DocDisplay,
                    DocName = x.DocName,
                    DocType = x.DocType,
                    Id = x.Id
                }).ToArray(),
            MailingList = x.MailingList
        }) .ToArrayAsync().ConfigureAwait(false);
         return allExemptionNotices;
    }
    #region AddDeleteUpdate
    public async Task<ExemptionNotices> AddExemptionNoticesAsync(ExemptionNotices exemptionNotice)
    {
        ExemptionNotices exemptionNotices = await _context.ExemptionNotices
            .Where(i => i.Id == exemptionNotice.Id).SingleOrDefaultAsync();

        if (exemptionNotices != null)
        {
            return null;
        }
        EntityEntry<ExemptionNotices> entity = _context.ExemptionNotices.Add(exemptionNotice);
        ExemptionNotices newExemptionNotice = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.ExemptionNotices);
            _ = await UpdateCacheExemptionNoticesAsync();
            return newExemptionNotice;
        }
        return newExemptionNotice;
    }

    public async Task<bool> DeleteExemptionNoticesAsync(Guid Id)
    {
        ExemptionNotices item = await _context.ExemptionNotices.SingleOrDefaultAsync(p => p.Id == Id);
        if (item != null)
        {
            IEnumerable<TenderDocuments> docs = _context.TenderDocuments.Where(x => x.ExemptionNoticesId == Id);
            if (docs != null)
            {
                _context.TenderDocuments.RemoveRange(docs.First(), docs.Last());
            }
        }
        _ = _context.ExemptionNotices.Remove(item);
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.ExemptionNotices);
            _ = await UpdateCacheExemptionNoticesAsync();
        }
        return success;
    }
    public async Task<ExemptionNotices> UpdateExemptionNoticesAsync(ExemptionNotices exemptionNoticesToUpdate)
    {
        ExemptionNotices exemptionNotices = await _context.ExemptionNotices.SingleOrDefaultAsync(p => p.Id == exemptionNoticesToUpdate.Id);

        if (exemptionNotices != null)
        {
            _ = _context.ExemptionNotices.Attach(exemptionNotices);
            _ = _context.ExemptionNotices.Remove(exemptionNotices);

            EntityEntry<ExemptionNotices> entity = _context.ExemptionNotices.Attach(exemptionNoticesToUpdate);

            _context.Entry(exemptionNoticesToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {

                exemptionNotices = entity.Entity;
                await _cacheService.RemoveCacheItemAsync(CacheKeys.ExemptionNotices);
                _ = await UpdateCacheExemptionNoticesAsync();
            }
        }

        return exemptionNotices;
    }
    #endregion AddDeleteUpdate
}
