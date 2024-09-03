using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
using AzureCacheForUmbraco.Enums;
//using Umbraco.Headless.Client.Net.Delivery;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureUmbracoCache
{

    public  class UmbracoCache
    {
   //     private static ILogger _logger;
        //private static IDistributedCache _distributedCache;
        private readonly IRedisCacheAccessor _redisCacheAccessor;

        public UmbracoCache(IRedisCacheAccessor
                redisCacheAccessor)
        {
            _redisCacheAccessor = redisCacheAccessor;
        }

        [FunctionName("UmbracoCache")]
        // "timerTrigger": "0 */2 * * * *"
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function)] HttpRequest req/*, ILogger log*/)
        {
            //_logger = log;
            //_distributedCache = distributedCache;
            //  CacheConfig _cacheConfig =  Environment.GetEnvironmentVariable("CacheSettings"); 

            //log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
           

            //try
            //{
            //    if ( GetUmbracoDataAsync().ConfigureAwait(false))
            //    {
            //        _logger.LogInformation("Processing umbraco cache setting endded succesfully");
            //        return true;
            //    }
            //    else
            //    {
            //        _logger.LogInformation("Processing umbraco cache setting  endded with failers");
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError("error occured", ex);
            //    throw;
            //}
            return new OkObjectResult(true);
        }

        private async Task<bool> GetUmbracoDataAsync(HttpRequest req)
        {

            CacheKeys[] keys = { CacheKeys.EnUs, CacheKeys.He, CacheKeys.Ar, CacheKeys.Ru };
            string culture;
            string umbracoUrl = Environment.GetEnvironmentVariable("UmbracoUrl");
            foreach (var key in keys)
            {
                switch (key)
                {
                    case CacheKeys.EnUs:
                        culture = "en-us";
                        break;
                    case CacheKeys.He:
                        culture = "he";
                        break;
                    case CacheKeys.Ru:
                        culture = "ru";
                        break;
                    case CacheKeys.Ar:
                        culture = "ar";
                        break;
                    default:
                        culture = "he";
                        break;
                }
                await RemoveCacheItemAsync(key,req);
            //    IEnumerable<FilteredContent> content = await GetContent(culture, umbracoUrl);

           //     await SetAsync<IEnumerable<FilteredContent>>(key, content).ConfigureAwait(false);

            }
            return true;
        }

        public async Task<bool> RemoveCacheItemAsync(CacheKeys cacheKey, HttpRequest req)
        {
          //  string key = Enum.GetName(typeof(CacheKeys), cacheKey);
            var reqHeaders = req.Headers;
          //  string cacheKey = reqHeaders["cacheKey"];
          //  string cacheValue = reqHeaders["cacheValue"];
            string cacheConnectionString = Environment.GetEnvironmentVariable("redisCacheConnectionString");
            string retValue = _redisCacheAccessor.ReadWriteFromCache(cacheConnectionString, req.Method, cacheKey.ToString(), "");
            return true;
            // return _distributedCache.RemoveAsync(key);
        }

        //private static async Task<IEnumerable<FilteredContent>> GetContent(string culture, string umbracoUrl)
        //{
        //    ContentDeliveryService m = new ContentDeliveryService(umbracoUrl);
        //    var content = await m.Content.GetRoot(culture);
        //    List<FilteredContent> arr = new List<FilteredContent>();
        //    foreach (var item in content)
        //    {
        //        arr.Add(new FilteredContent(item.Id, item.Name, item.Properties, item.Url));
        //    }

        //    return arr;
        //}

        //public static async Task SetAsync<T>(CacheKeys key, T value)
        //{
        //    string json = JsonConvert.SerializeObject(value);

        //    string cacheKey = Enum.GetName(typeof(CacheKeys), key);
        //    int AbsoluteExpiration = Int32.Parse(Environment.GetEnvironmentVariable("AbsoluteExpiration"));
        //    int SlidingExpiration = Int32.Parse(Environment.GetEnvironmentVariable("SlidingExpiration"));

        //    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
        //    {
        //        AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(AbsoluteExpiration),
        //        SlidingExpiration = TimeSpan.FromMinutes(SlidingExpiration)
        //    };

        //    await _distributedCache.SetStringAsync(cacheKey, json, options);
        //    _logger.LogInformation($"Cache item [{cacheKey}] was set succesfully");
        //}
    }
}

