using Core.Enums;

namespace Core.Interfaces;

public interface ICacheService
{
    Task<T> GetAsync<T>(CacheKeys key);
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(CacheKeys key, T value);
    Task SetAsync<T>(string key, T value);
    Task<IEnumerable<string>> GetCacheKeysAsync(string connectionString);
    Task RemoveCacheItemAsync(CacheKeys cacheKey);
    Task RemoveCacheItemAsync(string cacheKey);
 }
