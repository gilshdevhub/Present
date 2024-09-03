using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using AzureCacheForUmbraco.Entities.FilteredContent;
using AzureCacheForUmbraco.Enums;
using Umbraco.Headless.Client.Net.Delivery;
using Newtonsoft.Json;
using AzureCacheForUmbraco.Entities.Config;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace AzureCacheForUmbraco
{

    public static class UmbracoCache
    {
        private static ILogger _logger;
    
        

        [FunctionName("UmbracoCache")]
        // "timerTrigger": "0 */2 * * * *"
        public static async Task Run([TimerTrigger("%timerTrigger%")] TimerInfo myTimer, ILogger log)
        {
            _logger = log;
            //  CacheConfig _cacheConfig =  Environment.GetEnvironmentVariable("CacheSettings"); 

            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


            try
            {
                if (await GetUmbracoDataAsync(_logger).ConfigureAwait(false))
                {
                    _logger.LogInformation("Processing umbraco cache setting endded succesfully");
                }
                else
                {
                    _logger.LogInformation("Processing umbraco cache setting  endded with failers");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("error occured1", ex);
            }
        }

        private static async Task<bool> GetUmbracoDataAsync(ILogger log)
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
                        _logger.LogInformation($"culture is{culture}");
                        break;
                    case CacheKeys.He:
                        culture = "he";
                        _logger.LogInformation($"culture is{culture}");
                        break;
                    case CacheKeys.Ru:
                        culture = "ru";
                        _logger.LogInformation($"culture is{culture}");
                        break;
                    case CacheKeys.Ar:
                        culture = "ar";
                        _logger.LogInformation($"culture is{culture}");
                        break;
                    default:
                        culture = "he";
                        break;
                }
                _logger.LogInformation($"before GetContent");
                IEnumerable <FilteredContent> content = await GetContent(culture, umbracoUrl);
                _logger.LogInformation($"get content correctly worked");
           //     _logger.LogInformation($"content: {content}");
                try
                {
                    if (await SetUmbracoCache(key, content,log).ConfigureAwait(false))
                    {
                        _logger.LogInformation($"{key} saved correctly");
                    }
                    else
                    {
                        _logger.LogInformation($"{key} didn't save correctly");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("error occured", ex);
                }
                
            }
            return true;
        }

        private static async Task<IEnumerable<FilteredContent>> GetContent(string culture, string umbracoUrl)
        {
            ContentDeliveryService m = new ContentDeliveryService(umbracoUrl);
            var content = await m.Content.GetRoot(culture);
            List<FilteredContent> arr = new List<FilteredContent>();
            foreach (var item in content)
            {
                arr.Add(new FilteredContent(item.Id, item.Name, item.Properties, item.Url));
            }

            return arr;
        }

        public static async Task<bool> SetUmbracoCache(CacheKeys key, IEnumerable<FilteredContent> content, ILogger log)
        {
            string apiUrl = $"{Environment.GetEnvironmentVariable("ApiUrl")}/setCache?cacheKey={key}";
            string body = JsonConvert.SerializeObject(content);
         //   _logger.LogInformation($"Body: {body}");
            _logger.LogInformation($"apiUrl: {apiUrl}");
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("SubscriptionKey"));
           
            using (StringContent contentString = new StringContent(content: body, encoding: Encoding.UTF8, mediaType: "application/json"))
            using (HttpResponseMessage response = await httpClient.PostAsync(apiUrl, contentString).ConfigureAwait(false))
            {
                _logger.LogInformation($"{response}");
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
          //      var apiResponse = JsonConvert.DeserializeObject<AutomationNotificationResponse>(result);

                //if (apiResponse.SuccessStatus == 1)
                //{
                //    notificationEvents = apiResponse.Result;
                //}
                //else
                //{
                //    _logger.LogError("process automation notification failed", apiResponse.ErrorMessages);
                //}
            }

            return true;
        }
    }
}

