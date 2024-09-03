using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ManagmentSystemObjectsService : IManagmentSystemObjects
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    public ManagmentSystemObjectsService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }
    public async Task<List<ManagmentSystemObjects>> GetManagmentObjectsAsync()
    {
        List<ManagmentSystemObjects> managmentSystemObjects = await _context.ManagmentSystemObjects.ToListAsync().ConfigureAwait(false);
        return managmentSystemObjects;
    }

    public async Task<ManagmentSystemObjects?> GetManagmentObjectsByIdAsync(int Id)
    {
        return await _context.ManagmentSystemObjects.FirstOrDefaultAsync(x => x.Id == Id).ConfigureAwait(false);
    }

    public async Task<ManagmentSystemObjects?> GetManagmentObjectsByNameAsync(string Name)
    {
        return await _context.ManagmentSystemObjects.FirstOrDefaultAsync(x => x.Name == Name).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ManagmentSystemObjects>> GetManagmentSystemObjectsAsync()
    {
        IEnumerable<ManagmentSystemObjects> allObjects = null;// await _cacheService.GetAsync<IEnumerable<ManagmentSystemObjects>>(CacheKeys.ManagmentSystemObjects).ConfigureAwait(false);
        if (allObjects == null || allObjects.Count() <= 0)
        {
            allObjects = await _context.ManagmentSystemObjects.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.ManagmentSystemObjects, allObjects).ConfigureAwait(false);
        }
        return allObjects;
    }

    public async Task<ManagmentSystemObjects> UpdateManagmentObjectsByIdAsync(int Id, string value)
    {
        ManagmentSystemObjects systemObjects = (await GetManagmentSystemObjectsAsync().ConfigureAwait(false)).SingleOrDefault(p => p.Id == Id);
        systemObjects.StringValue = value;
        if (systemObjects != null)
        {
            _ = _context.ManagmentSystemObjects.Update(systemObjects);
            _context.Entry(systemObjects).State = EntityState.Modified;
            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.ManagmentSystemObjects);
                _ = await GetManagmentSystemObjectsAsync().ConfigureAwait(false);
            }
            return systemObjects;
        }
        return null;
    }
}
