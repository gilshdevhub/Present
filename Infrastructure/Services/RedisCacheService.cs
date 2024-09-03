using Core.Config;
using Core.Enums;
using Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.Azure.StackExchangeRedis;
using StackExchange.Redis;
using System.Net;

namespace Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly CacheConfig _cacheConfig;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IConfiguration _configuration;
    public RedisCacheService(IDistributedCache distributedCache, IOptions<CacheConfig> cacheConfig, ILogger<RedisCacheService> logger, IConfiguration configuration)
    {
        _distributedCache = distributedCache;
        _cacheConfig = cacheConfig.Value;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<T> GetAsync<T>(CacheKeys key)
    {
        string cacheKey = Enum.GetName(typeof(CacheKeys), key);

        string json = await _distributedCache.GetStringAsync(cacheKey).ConfigureAwait(false);
        if (json == null)
            return default;

        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<T> GetAsync<T>(string cacheKey)
    {
      
        string json = await _distributedCache.GetStringAsync(cacheKey).ConfigureAwait(false);
        if (json == null)
            return default;

        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task SetAsync<T>(string cacheKey, T value)
    {
        string json = JsonConvert.SerializeObject(value);

//        string cacheKey = Enum.GetName(typeof(CacheKeys), key);

        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_cacheConfig.AbsoluteExpiration),
            SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpiration)
        };

        await _distributedCache.SetStringAsync(cacheKey, json, options);
        _logger.LogInformation($"Cache item [{cacheKey}] was set succesfully");
    }



    public async Task SetAsync<T>(CacheKeys key, T value)
    {
        string json = JsonConvert.SerializeObject(value);

        string cacheKey = Enum.GetName(typeof(CacheKeys), key);

        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_cacheConfig.AbsoluteExpiration),
            SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpiration)
        };

        await _distributedCache.SetStringAsync(cacheKey, json, options);
        _logger.LogInformation($"Cache item [{cacheKey}] was set succesfully");
    }


    public async Task<T> GetStringAsync<T>(CacheKeys key)
    {
        string cacheKey = Enum.GetName(typeof(CacheKeys), key);
        string result = string.Empty;
        string connectionString = _configuration.GetConnectionString("azureCache");
        var options = ConfigurationOptions.Parse(connectionString);

        string managedIdentityClientId = _configuration.GetSection("managedIdentityClientId").Value == null ? "" : _configuration.GetSection("managedIdentityClientId").Value;
        if (managedIdentityClientId != null)
        {
            await options.ConfigureForAzureWithUserAssignedManagedIdentityAsync(managedIdentityClientId);
        }
        using ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync(options).ConfigureAwait(false);
        IDatabase cache = connection.GetDatabase();
        var val = await cache.StringGetAsync(cacheKey).ConfigureAwait(false);
        if (val.HasValue)
            return JsonConvert.DeserializeObject<T>(val);
        else
            return default;
       
    }



    public async Task<IEnumerable<string>> GetCacheKeysAsync(string connectionString)
    {
        ConfigurationOptions options = ConfigurationOptions.Parse(connectionString);

        using ConnectionMultiplexer connection = await ConnectionMultiplexer.ConnectAsync(options).ConfigureAwait(false);

        _ = connection.GetDatabase();

        EndPoint endPoint = connection.GetEndPoints().First();
        RedisKey[] redisKeys = connection.GetServer(endPoint).Keys(pattern: "*").ToArray();

        IEnumerable<string> keys = redisKeys.Select(p => p.ToString()).ToArray();
        return keys;
    }

    public Task RemoveCacheItemAsync(CacheKeys cacheKey)
    {
        string key = Enum.GetName(typeof(CacheKeys), cacheKey);
        return _distributedCache.RemoveAsync(key);
    }
    public Task RemoveCacheItemAsync(string key)
    {
        return _distributedCache.RemoveAsync(key);
    }
}
