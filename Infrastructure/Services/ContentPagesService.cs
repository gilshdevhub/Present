using Core.Entities.ContentPages;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class ContentPagesService : IContentPagesService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public ContentPagesService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<ContentPages> GetContentPageAsync(int id)
    {
        IEnumerable<ContentPages> contentPages = await GetCacheContentPagesAsync().ConfigureAwait(false);
        ContentPages contentPage = contentPages.SingleOrDefault(p => p.Id == id);
        return contentPage;
    }

    public async Task<IEnumerable<ContentPages>> GetContentPagesAsync()
    {
        IEnumerable<ContentPages> contentPages = await GetCacheContentPagesAsync().ConfigureAwait(false);
        return contentPages;
    }

    public async Task<ContentPages> AddContentPageAsync(ContentPages contentPageToAdd)
    {
        ContentPages contentPage = null;

        EntityEntry<ContentPages> entity = await _context.ContentPages.AddAsync(contentPageToAdd).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            contentPage = entity.Entity;
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ContentPages);
        }

        return contentPage;
    }

    public async Task<ContentPages> UpdateContentPageAsync(ContentPages contentPageToUpdate)
    {
        ContentPages contentPage = await _context.ContentPages
            .SingleOrDefaultAsync(p => p.Id == contentPageToUpdate.Id).ConfigureAwait(false);

        if (contentPage != null)
        {
            _ = _context.ContentPages.Remove(contentPage);
            _ = _context.ContentPages.Attach(contentPage);

            EntityEntry<ContentPages> entity = _context.ContentPages.Attach(contentPageToUpdate);

            _context.Entry(contentPageToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ContentPages);
                contentPage = entity.Entity;
            }
        }

        return contentPage;
    }

    public async Task<bool> DeleteContentPageAsync(int Id)
    {
        ContentPages contentPage = await _context.ContentPages.SingleOrDefaultAsync(p => p.Id == Id).ConfigureAwait(false);
        bool success = false;
        if (contentPage != null)
        {
            _ = _context.ContentPages.Remove(contentPage);
            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ContentPages);
            }
        }
        return success;
    }

    private async Task<IEnumerable<ContentPages>> GetCacheContentPagesAsync()
    {
        IEnumerable<ContentPages> contentPages = await _cacheService.GetAsync<IEnumerable<ContentPages>>(CacheKeys.ContentPages).ConfigureAwait(false);

        if (contentPages == null || contentPages.Count() <= 0)
        {
            contentPages = await _context.ContentPages.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<ContentPages>>(CacheKeys.ContentPages, contentPages).ConfigureAwait(false);
        }

        return contentPages;
    }
}
