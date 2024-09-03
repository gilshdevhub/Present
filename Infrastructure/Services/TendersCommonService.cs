using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TendersCommonService : ITendersCommonService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public TendersCommonService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<TenderDocuments>> GetTenderDocumentsContentAsync()
    {
        IEnumerable<TenderDocuments> allTenderDocuments = await _cacheService.GetAsync<IEnumerable<TenderDocuments>>(CacheKeys.TenderDocuments).ConfigureAwait(false);
        if (allTenderDocuments == null || allTenderDocuments.Count() <= 0)
        {
            allTenderDocuments = await _context.TenderDocuments.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.TenderDocuments, allTenderDocuments).ConfigureAwait(false);
        }

        return allTenderDocuments;
    }
}
