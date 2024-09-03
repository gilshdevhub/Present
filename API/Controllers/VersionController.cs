using Core.Entities.VersionCatalog;
using Core.Enums;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class VersionController : BaseApiController
{
    private readonly IVersioningService _versioningService;

    public VersionController(IVersioningService versioningService)
    {
        _versioningService = versioningService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VersionCatalog>>> GetVersions()
    {
        IEnumerable<VersionCatalog> versionCatalogs = await _versioningService.GetVersionsAsync().ConfigureAwait(false);
        return Ok(versionCatalogs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VersionCatalog>> GetVersions(Versioning id)
    {
        VersionCatalog versionCatalog = await _versioningService.GetVersionAsync(id).ConfigureAwait(false);
        return Ok(versionCatalog);
    }
}
