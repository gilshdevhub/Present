using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
//using Core.Entities.Umbraco;
//using Core.Enums;
//using Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;


namespace AzureUmbracoCache
{
  
    public static class UmbracoCache
    {
       
        private static ILogger _log;
       
        [FunctionName("UmbracoCache")]
        public static async Task Run([TimerTrigger("%timerTrigger%")] TimerInfo myTimer, ILogger log)
        {
            _log = log;

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            //   _cacheService = cacheService;
            //   _umbracoService = umbracoService;
            //   CacheKeys[] keys =  { CacheKeys.EnUs, CacheKeys.He, CacheKeys.Ar, CacheKeys.Ru };
            //   string culture;
            //   string umbracoUrl = Environment.GetEnvironmentVariable("UmbracoUrl");
            ////   string umbracoUrl = _configuration.GetSection("UmbracoUrl").Value;
            //   foreach (var key in keys)
            //   {
            //       switch (key)
            //       {
            //           case CacheKeys.EnUs:
            //               culture = "en-us";
            //               break;
            //           case CacheKeys.He:
            //               culture = "he";
            //               break;
            //           case CacheKeys.Ru:
            //               culture = "ru";
            //               break;
            //           case CacheKeys.Ar:
            //               culture = "ar";
            //               break;
            //           default:
            //               culture = "he";
            //               break;
            //       }
            //       await _cacheService.RemoveCacheItemAsync(key);
            //       IEnumerable<FilteredContent> content = await _umbracoService.GetContent(culture, umbracoUrl);

            //       await _cacheService.SetAsync<IEnumerable<FilteredContent>>(key, content).ConfigureAwait(false);
            //}
            var a = 1;
            return;
        }
    }
}
