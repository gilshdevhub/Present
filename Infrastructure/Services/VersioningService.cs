using Core.Entities.VersionCatalog;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class VersioningService : IVersioningService
{
    private readonly RailDbContext _railDbContext;

    public VersioningService(RailDbContext railDbContext)
    {
        _railDbContext = railDbContext;
    }

    public async Task<IEnumerable<VersionCatalog>> GetVersionsAsync()
    {
        return await _railDbContext.VersionCatalog.ToArrayAsync().ConfigureAwait(false);
    }

    public Task<VersionCatalog> GetVersionAsync(Versioning id)
    {
        string versionName = Enum.GetName(typeof(Versioning), id);
        return _railDbContext.VersionCatalog.SingleOrDefaultAsync(p => p.Description.ToLower().Equals(versionName.ToLower()));
    }
}
