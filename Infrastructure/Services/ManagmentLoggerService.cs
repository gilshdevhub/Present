using Core.Entities.ManagmentLogger;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ManagmentLoggerService : IManagmentLogger
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public ManagmentLoggerService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;

    }
    public async Task<bool> DeleteLogAsync(int Id)
    {
        bool success = false;
        ManagmentLog item = await _context.ManagmentLog.SingleOrDefaultAsync(p => p.Id == Id).ConfigureAwait(false);
        _ = _context.ManagmentLog.Remove(item);
        success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            IEnumerable<ManagmentLog> AllManagmentLogs = await _context.ManagmentLog.ToArrayAsync().ConfigureAwait(false);

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ManagmentLog);
            await _cacheService.SetAsync<IEnumerable<ManagmentLog>>(CacheKeys.ManagmentLog, AllManagmentLogs).ConfigureAwait(false);
        }
        return success;
    }

    public async Task<IEnumerable<ManagmentLog>> GetAllLogAsync()
    {
        IEnumerable<ManagmentLog> AllManagmentLogs = (await GetCacheManagmentLogAsync().ConfigureAwait(false));
        return AllManagmentLogs;
    }


    public async Task<ManagmentLog> AddLogAsync(ManagmentLog item)
    {
        item.LogTime = DateTime.Now;
        var entity = _context.ManagmentLog.Add(item);
        ManagmentLog newManagmentLog = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;

        if (success)
        {
            IEnumerable<ManagmentLog> AllManagmentLog = await _context.ManagmentLog.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ManagmentLog);
            await _cacheService.SetAsync<IEnumerable<ManagmentLog>>(CacheKeys.ManagmentLog, AllManagmentLog).ConfigureAwait(false);
            return newManagmentLog;
        }
        else
        {
            return null;
        }
    }
    private async Task<IEnumerable<ManagmentLog>> GetCacheManagmentLogAsync()
    {
        IEnumerable<ManagmentLog> AllManagmentLogs = await _cacheService.GetAsync<IEnumerable<ManagmentLog>>(CacheKeys.ManagmentLog).ConfigureAwait(false);

        if (AllManagmentLogs == null || AllManagmentLogs.Count() <= 0)
        {
            AllManagmentLogs = await _context.ManagmentLog.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<ManagmentLog>>(CacheKeys.ManagmentLog, AllManagmentLogs).ConfigureAwait(false);
        }

        return AllManagmentLogs;
    }
}
