using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureAutomationNotifications.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureAutomationNotifications
{
    public static class AutomationNotifications
    {
        private static ILogger _logger;

        [FunctionName("AutomationNotifications")]
        // "timerTrigger_Automation": "0 */2 * * * *"
        public static async Task Run([TimerTrigger("%timerTrigger_Automation%")]TimerInfo myTimer, ILogger log)
        {
            _logger = log;

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogError("job ended with error - connection string is empty");
                return;
            }

            try
            {
                IEnumerable<AutomationNotification> automationNotifications = await GetAutomationNotifications(connectionString).ConfigureAwait(false);
                if (automationNotifications.Any())
                {
                    int notificationEvents = await ProcessAutomationNotificationsAsync(automationNotifications).ConfigureAwait(false);
                    _logger.LogInformation($"total of {notificationEvents} notification evnts created");
                }
            }
            catch(Exception ex)
            {
                log.LogError(ex, "error occured!");
            }
        }

        private static async Task<IEnumerable<AutomationNotification>> GetAutomationNotifications(string connectionString)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[notifications_GetAutomationNotifications]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = conn;
                    dataTable.Load(await command.ExecuteReaderAsync().ConfigureAwait(false));
                }
            }

            List<AutomationNotification>  automationNotifications = dataTable.AsEnumerable().Select(dr => new AutomationNotification
            {
                Id = dr.Field<int>("Id"),
                CreateDate = dr.Field<DateTime>("CreateDate"),
                TrainNumber = dr.Field<int>("TrainNumber"),
                TrainDate = dr.Field<DateTime>("TrainDate"),
                NotificationTypeId = dr.Field<int>("NotificationTypeId"),
                ChangedStationId = dr.Field<int?>("ChangedStationId"),
                ChangedPlatformId = dr.Field<int?>("ChangedPlatformId"),
                ChangedTrainTime = dr.Field<DateTime?>("ChangedTrainTime")
            }).ToList();

            _logger.LogInformation($"retrieved {automationNotifications.Count} records from table AutomationNotifications");

            return automationNotifications.ToArray();
        }
        private static async Task<int> ProcessAutomationNotificationsAsync(IEnumerable<AutomationNotification> automationNotifications)
        {
            int notificationEvents = 0;

            string apiUrl = $"{Environment.GetEnvironmentVariable("ApiUrl")}/automations";
            string body = System.Text.Json.JsonSerializer.Serialize(automationNotifications);

            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("SubscriptionKey"));

            using (StringContent content = new StringContent(content: body, encoding: Encoding.UTF8, mediaType: "application/json"))
            using (HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                AutomationNotificationResponse apiResponse = JsonConvert.DeserializeObject<AutomationNotificationResponse>(result);

                if (apiResponse.SuccessStatus == 1)
                {
                    notificationEvents = apiResponse.Result;
                }
                else
                {
                    _logger.LogError("process automation notification failed", apiResponse.ErrorMessages);
                }
            }

            return notificationEvents;
        }
    }
}
