using ContentManagement.Controllers;
using Core.Entities.Umbraco;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UmbracoCache;

public class UmbracoCacheController : BaseApiController
{
    private static ICacheService _cacheService;
    private static IUmbracoService _umbracoService;
    private readonly IConfiguration _configuration;
    public UmbracoCacheController(IConfiguration configuration, ICacheService cacheService, IUmbracoService umbracoService)
    {
        _configuration = configuration;
        _cacheService = cacheService;
        _umbracoService = umbracoService;
    }

    [HttpPost("setCache")]
    public async Task<ActionResult> SetUmbracoCache(CacheKeys cacheKey, IEnumerable<FilteredContent> content)
    {
        await _cacheService.RemoveCacheItemAsync(cacheKey);
        await _cacheService.SetAsync<IEnumerable<FilteredContent>>(cacheKey, content).ConfigureAwait(false);
        return Ok();
    }
}


