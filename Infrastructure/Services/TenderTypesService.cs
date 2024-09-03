using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class TenderTypesService : ITenderTypesService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    public TenderTypesService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }


    public async Task<TenderTypes?> GetTenderTypeByIdAsync(int Id)
    {
        TenderTypes? tenderType = (await GetTenderTypesContentAsync().ConfigureAwait(false)).SingleOrDefault(x => x.Id == Id);

        return tenderType;
    }

    public async Task<TenderTypes?> GetTenderTypeByIdNoCache(int Id)
    {
        TenderTypes? tenderType = (await GetTenderTypesContentNoCache().ConfigureAwait(false)).SingleOrDefault(x => x.Id == Id);

        return tenderType;
    }

    public async Task<IEnumerable<TenderTypes>> GetTenderTypesAsync()
    {
        IEnumerable<TenderTypes> tenderTypes = await GetTenderTypesContentAsync().ConfigureAwait(false);

        return tenderTypes;
    }

    public async Task<IEnumerable<TenderTypes>> GetTenderTypesNoCache()
    {
        IEnumerable<TenderTypes> tenderTypes = await GetTenderTypesContentNoCache().ConfigureAwait(false);

        return tenderTypes;
    }

    private async Task<IEnumerable<TenderTypes>> GetTenderTypesContentAsync()
    {
        IEnumerable<TenderTypes> allTenderTypes = await _cacheService.GetAsync<IEnumerable<TenderTypes>>(CacheKeys.TenderTypes).ConfigureAwait(false);
        if (allTenderTypes == null || allTenderTypes.Count() <= 0)
        {
            allTenderTypes = await GetTenderTypesContentNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.TenderTypes, allTenderTypes).ConfigureAwait(false);
        }

        return allTenderTypes;
    }

    private async Task<IEnumerable<TenderTypes>> GetTenderTypesContentNoCache()
    {
        IEnumerable<TenderTypes> allTenderTypes = await _context.TenderTypes.ToArrayAsync().ConfigureAwait(false);

        return allTenderTypes;
    }
    #region AddDeleteUpdate
    public async Task<TenderTypes?> AddTenderTypesAsync(TenderTypes tenderType)
    {
        TenderTypes? tenderTypes = (await GetTenderTypesContentAsync().ConfigureAwait(false))
           .Where(i => i.Id == tenderType.Id).SingleOrDefault();

        if (tenderTypes != null)
        {
            return null;
        }

        EntityEntry<TenderTypes> entity = _context.TenderTypes.Add(tenderType);
        TenderTypes newTenderTypes = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.TenderTypes);
            IEnumerable<TenderTypes> allTenderTypes = await GetTenderTypesContentNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.TenderTypes, allTenderTypes).ConfigureAwait(false);
        }
        return newTenderTypes;
    }

    public async Task<bool> DeleteTenderTypesAsync(int Id)
    {
        TenderTypes? item = (await GetTenderTypesContentAsync().ConfigureAwait(false)).SingleOrDefault(p => p.Id == Id);
        _ = _context.TenderTypes.Remove(item);
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.TenderTypes);
            IEnumerable<TenderTypes> allTenderTypes = await GetTenderTypesContentNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.TenderTypes, allTenderTypes).ConfigureAwait(false);
        }
        return success;
    }

    public async Task<TenderTypes?> UpdateTenderTypesAsync(TenderTypes tenderTypeToUpdate)
    {
        TenderTypes? tenderTypes = (await GetTenderTypesContentAsync().ConfigureAwait(false)).SingleOrDefault(p => p.Id == tenderTypeToUpdate.Id);

        if (tenderTypes != null)
        {
            _ = _context.TenderTypes.Attach(tenderTypes);
            _ = _context.TenderTypes.Remove(tenderTypes);

            EntityEntry<TenderTypes> entity = _context.TenderTypes.Attach(tenderTypeToUpdate);

            _context.Entry(tenderTypeToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.TenderTypes);
                tenderTypes = entity.Entity;
                IEnumerable<TenderTypes> allTenderTypes = await GetTenderTypesContentNoCache().ConfigureAwait(false);
                await _cacheService.SetAsync(CacheKeys.TenderTypes, allTenderTypes).ConfigureAwait(false);
            }
        }

        return tenderTypes;
    }
    #endregion AddDeleteUpdate
}
