using Core.Entities.AppMessages;
using Core.Entities.Configuration;
using Core.Entities.Notifications;
using Core.Entities.RailUpdates;
using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SiteManagement.Controllers;

public class CacheController : BaseApiController
{
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public CacheController(ICacheService cacheService, IConfiguration configuration)
    {
        _cacheService = cacheService;
        _configuration = configuration;
    }

    [HttpGet("keys")]
    public async Task<ActionResult<IEnumerable<string>>> GetKeys()
    {
        string connectionString = _configuration.GetConnectionString("azureCache");

        if (string.IsNullOrEmpty(connectionString))
        {
            return BadRequest("invalid connection string to azure cache");
        }

        IEnumerable<string> keys = await _cacheService.GetCacheKeysAsync(connectionString).ConfigureAwait(false);
        return Ok(keys);
    }

    [HttpGet("{cacheKey}")]
    public async Task<ActionResult<object>> GetItem(CacheKeys cacheKey)
    {
        object cacheItem = null;

        switch (cacheKey)
        {
            case CacheKeys.ConfigurationParameter:
                cacheItem = await _cacheService.GetAsync<IEnumerable<ConfigurationParameter>>(cacheKey).ConfigureAwait(false);
                break;
            case CacheKeys.Stations:
                cacheItem = await _cacheService.GetAsync<IEnumerable<Station>>(cacheKey).ConfigureAwait(false);
                break;
            case CacheKeys.Translations:
                cacheItem = await _cacheService.GetAsync<IEnumerable<Translation>>(cacheKey).ConfigureAwait(false);
                break;
            case CacheKeys.NotificationTypes:
                cacheItem = await _cacheService.GetAsync<IEnumerable<NotificationType>>(cacheKey).ConfigureAwait(false);
                break;
            case CacheKeys.VoucherErrorCodes:
                cacheItem = await _cacheService.GetAsync<IEnumerable<VoucherErrorCode>>(cacheKey).ConfigureAwait(false);
                break;
            case CacheKeys.RailUpdates:
                cacheItem = await _cacheService.GetAsync<RailUpdate>(cacheKey).ConfigureAwait(false);
                break;
            case CacheKeys.PopUpMessages:
                cacheItem = await _cacheService.GetAsync<IEnumerable<PopUpMessages>>(cacheKey).ConfigureAwait(false);
                break;
        }

        return cacheItem == null ? NotFound($"item [{Enum.GetName(typeof(CacheKeys), cacheKey)}] is not in cache") : Ok(cacheItem);
    }

    [HttpDelete("{cacheKey}")]
    public async Task<ActionResult<bool>> RemoveCacheItem(CacheKeys cacheKey)
    {
        string cacheItemToDelete = Enum.GetName<CacheKeys>(cacheKey);
        string connectionString = _configuration.GetConnectionString("azureCache");

        if (string.IsNullOrEmpty  
            (connectionString))
        {
            return BadRequest(JsonSerializer.Serialize("invalid connection string to azure cache"));
        }

        IEnumerable<string> cacheItems = await _cacheService.GetCacheKeysAsync(connectionString).ConfigureAwait(false);
        if (!cacheItems.Contains(cacheItemToDelete))
        {
            return Ok(JsonSerializer.Serialize($"item {cacheItemToDelete} is not in cache"));
        }

        await _cacheService.RemoveCacheItemAsync(cacheKey).ConfigureAwait(false);
        return Ok(JsonSerializer.Serialize($"item [{cacheItemToDelete}] was removed from cache"));
    }

    [HttpDelete("clear")]
    public async Task<ActionResult<bool>> Clear()
    {
        string connectionString = _configuration.GetConnectionString("azureCache");

        if (string.IsNullOrEmpty(connectionString))
        {
            return BadRequest(JsonSerializer.Serialize("invalid connection string to azure cache"));
        }
               IEnumerable<string> cacheKeys = await _cacheService.GetCacheKeysAsync(connectionString).ConfigureAwait(false);

        if (!cacheKeys.Any())
        {
            return NotFound(JsonSerializer.Serialize("cache has no items"));
        }

        string itemsInCache = cacheKeys.Aggregate((x, y) => $"{x}, {y}");

        foreach (var cacheItem in cacheKeys)
        {
            try
            {

                CacheKeys cacheKey = Enum.Parse<CacheKeys>(cacheItem);
                await _cacheService.RemoveCacheItemAsync(cacheKey).ConfigureAwait(false);

            }
            catch (Exception ex) { }
        }

        return Ok(JsonSerializer.Serialize($"items removed from cache: {itemsInCache}"));
    }
    [HttpDelete("clearAll")]
    public async Task<ActionResult<bool>> ClearAll()
    {
        string connectionString = _configuration.GetConnectionString("azureCache");

        if (string.IsNullOrEmpty(connectionString))
        {
            return BadRequest(JsonSerializer.Serialize("invalid connection string to azure cache"));
        }

        IEnumerable<string> cacheKeys = await _cacheService.GetCacheKeysAsync(connectionString).ConfigureAwait(false);

        if (!cacheKeys.Any())
        {
            return NotFound(JsonSerializer.Serialize("cache has no items"));
        }

        string itemsInCache = cacheKeys.Aggregate((x, y) => $"{x}, {y}");

        foreach (var cacheItem in cacheKeys)
        {
            await _cacheService.RemoveCacheItemAsync(cacheItem).ConfigureAwait(false);
        }

        return Ok(JsonSerializer.Serialize($"items removed from cache: {itemsInCache}"));
    }
}
