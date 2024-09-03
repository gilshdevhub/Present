using Core.Entities.VersionCatalog;
using Core.Enums;

namespace Core.Interfaces;

public interface IVersioningService
{
    Task<IEnumerable<VersionCatalog>> GetVersionsAsync();
    Task<VersionCatalog> GetVersionAsync(Versioning id);
}
