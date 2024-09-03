using System;
using System.Net.Http;
using System.Threading.Tasks;
using AzureGeneralRailUpdates.Dtos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureGeneralRailUpdates
{
    public static class GeneralRailUpdates
    {
        [FunctionName("GeneralRailUpdates")]
        public static async Task Run([TimerTrigger("%timerTrigger%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string apiUrl = Environment.GetEnvironmentVariable("ApiUrl");

            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("SubscriptionKey"));

            using HttpResponseMessage response = await httpClient.PostAsync(apiUrl, null).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

            if (apiResponse.SuccessStatus == 1 && apiResponse.Result)
            {
                log.LogInformation("process general rail updates ended successfully");
            }
            else
            {
                log.LogError("process general rail updates failed", apiResponse.ErrorMessages);
            }
        }
    }
}
