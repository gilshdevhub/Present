using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureNotificationEvents.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureNotificationEvents
{
    public static class NotificationEvents
    {
        private static ILogger _logger;

        [FunctionName("NotificationEvents")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            _logger = log;

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogInformation("job ended with error - connection string is empty");
                return;
            }

            IEnumerable<NotificationEvent> notificationEvents = await GetNotificationEventsAsync(connectionString).ConfigureAwait(false);

            if (notificationEvents.Any())
            {
                if (await ProcessNotificationEventsAsync(notificationEvents).ConfigureAwait(false))
                {
                    _logger.LogInformation("Processing notification events endded succesfully");
                }
                else
                {
                    _logger.LogWarning("Processing notification events endded with failers");
                }
            }
        }

        private static async Task<IEnumerable<NotificationEvent>> GetNotificationEventsAsync(string connectionString)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[notifications_GetNotificationEvents]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    dataTable.Load(await command.ExecuteReaderAsync().ConfigureAwait(false));
                }
            }

            List<NotificationEvent> notificationEvents = dataTable.AsEnumerable().Select(dr => new NotificationEvent
            {
                Id = dr.Field<int>("Id"),
                CreateDate = dr.Field<DateTime>("CreateDate"),
                PushNotificationId = dr.Field<int>("PushNotificationId"),
                PushRegistrationId = dr.Field<int>("PushRegistrationId"),
                Message = dr.Field<string>("Message"),
                TimeToSend = dr.Field<DateTime>("TimeToSend"),
                NotificationTypeId = dr.Field<int>("NotificationTypeId"),
                Status = dr.Field<int>("Status"),
                AutomationNotificationId = dr.Field<int>("AutomationNotificationId")
            }).ToList();

            _logger.LogInformation($"retrieved {notificationEvents.Count} records from table NotificationEvents");

            return notificationEvents.ToArray();
        }
        private static async Task<bool> ProcessNotificationEventsAsync(IEnumerable<NotificationEvent> notificationEvents)
        {
            bool isSuccess = false;

            string apiUrl = Environment.GetEnvironmentVariable("ApiUrl");
            string body = System.Text.Json.JsonSerializer.Serialize(notificationEvents);

            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("SubscriptionKey"));

            using (StringContent content = new StringContent(content: body, encoding: Encoding.UTF8, mediaType: "application/json"))
            using (HttpResponseMessage response = await httpClient.PutAsync(apiUrl, content).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(result);

                if (apiResponse.SuccessStatus == 1)
                {
                    isSuccess = apiResponse.Result;
                }
                else
                {
                    _logger.LogError("process automation notification failed", apiResponse.ErrorMessages);
                }
            }

            return isSuccess;
        }
    }
}