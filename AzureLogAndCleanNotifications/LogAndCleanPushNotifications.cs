using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureLogAndCleanNotifications
{
    public static class LogAndCleanPushNotifications
    {
        private static ILogger _log;

        [FunctionName("LogAndCleanPushNotifications")]
        // "TimerTrigger_LogAndCleanPushNotifications": "0 5 2 * * *"
        public static async Task Run([TimerTrigger("%TimerTrigger_LogAndCleanPushNotifications%")]TimerInfo myTimer, ILogger log)
        {
            _log = log;

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogError("job ended with error - connection string is empty");
                return;
            }

            await LogPushNotificationsAsync(connectionString).ConfigureAwait(false);
            log.LogInformation($"End LogAndCleanPushNotifications function: {DateTime.Now}");
        }

        private static async Task LogPushNotificationsAsync(string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand();
                command.CommandText = "[dbo].[notifications_LogPushNotifications]";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter tranCount = command.Parameters.AddWithValue("@tranCount", SqlDbType.Int);
                tranCount.Direction = ParameterDirection.ReturnValue;

                command.Connection = conn;

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                _log.LogInformation((int)tranCount.Value == 0 ? "SUCCESS - LogPushNotificationsAsync ended succesfully" : "FAIL - LogPushNotificationsAsync ended with error");
            }
            catch (Exception ex)
            {
                _log.LogError($"FAIL - LogPushNotificationsAsync ended with fail: {ex.Message}");
            }
        }
    }
}
