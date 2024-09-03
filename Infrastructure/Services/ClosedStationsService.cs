using Core.Entities.Stations;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ClosedStationsService : IClosedStationsService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    public ClosedStationsService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }
    public async Task<ActionResult<bool>> DeleteClosedStationsAndLinesAsync(int Id)
    {
        bool success = false;
        ClosedStationsAndLines item = await _context.ClosedStationsAndLines.SingleOrDefaultAsync(p => p.Id == Id).ConfigureAwait(false);
        _ = _context.ClosedStationsAndLines.Remove(item);
        success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            IEnumerable<ClosedStationsAndLines> closedStationsAndLines = await _context.ClosedStationsAndLines.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ClosedStationsAndLines);
            await _cacheService.SetAsync<IEnumerable<ClosedStationsAndLines>>(CacheKeys.ClosedStationsAndLines, closedStationsAndLines).ConfigureAwait(false);
        }
        return success;
    }

    public async Task<ActionResult<ClosedStationsAndLines>> GetClosedStationsAndLinesByIdAsync(int Id)
    {
        ClosedStationsAndLines closedStationsAndLine = (await GetCacheClosedStationsAndLinesAsync().ConfigureAwait(false)).FirstOrDefault(x => x.Id == Id);
        return closedStationsAndLine;
    }

    public async Task<IEnumerable<ClosedStationsAndLines>> GetClosedStationsAndLinesAsync()
    {
        IEnumerable<ClosedStationsAndLines> closedStationsAndLines = await GetCacheClosedStationsAndLinesAsync().ConfigureAwait(false);
        return closedStationsAndLines;
    }

    public async Task<bool> UpdateClosedStationsAndLinesAsync(ClosedStationsAndLines stationToUpdate)
    {
        _ = _context.ClosedStationsAndLines.Update(stationToUpdate);
        _context.Entry(stationToUpdate).State = EntityState.Modified;
        bool success = await _context.SaveChangesAsync() > 0;

        if (success)
        {
            IEnumerable<ClosedStationsAndLines> closedStationsAndLines = await _context.ClosedStationsAndLines.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ClosedStationsAndLines);
            await _cacheService.SetAsync<IEnumerable<ClosedStationsAndLines>>(CacheKeys.ClosedStationsAndLines, closedStationsAndLines).ConfigureAwait(false);
        }
        return success;
    }
    private async Task<IEnumerable<ClosedStationsAndLines>> GetCacheClosedStationsAndLinesAsync()
    {
        IEnumerable<ClosedStationsAndLines> closedStationsAndLines = await _cacheService.GetAsync<IEnumerable<ClosedStationsAndLines>>(CacheKeys.ClosedStationsAndLines).ConfigureAwait(false);

        if (closedStationsAndLines == null || closedStationsAndLines.Count() <= 0)
        {
            closedStationsAndLines = await _context.ClosedStationsAndLines.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<ClosedStationsAndLines>>(CacheKeys.ClosedStationsAndLines, closedStationsAndLines).ConfigureAwait(false);
        }

        return closedStationsAndLines;
    }

    public async Task<ClosedStationsAndLines> AddClosedStationsAndLinesAsync(ClosedStationsAndLines stationToUpdate)
    {
        ClosedStationsAndLines closedStationsAndLines = await _context.ClosedStationsAndLines.SingleOrDefaultAsync(i => i.Id == stationToUpdate.Id);

        if (closedStationsAndLines != null)
        {
            return null;
        }

        var entity = _context.ClosedStationsAndLines.Add(stationToUpdate);
        ClosedStationsAndLines newClosedStations = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;

        if (success)
        {
            IEnumerable<ClosedStationsAndLines> closedStationsArray = await _context.ClosedStationsAndLines.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.ClosedStationsAndLines);
            await _cacheService.SetAsync<IEnumerable<ClosedStationsAndLines>>(CacheKeys.ClosedStationsAndLines, closedStationsArray).ConfigureAwait(false);
            return newClosedStations;
        }
        else
        {
            return null;
        }
    }

                                                         }
