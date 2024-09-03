using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureLogAndCleanNotifications
{
    public static class CleanAutomationNotifications
    {
        private static ILogger _log;

        [FunctionName("CleanAutomationNotifications")]
        //  "TimerTrigger_CleanAutomationNotifications": "0 10 2 * * *"
        public static async Task Run([TimerTrigger("%TimerTrigger_CleanAutomationNotifications%")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            _log = log;

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogError("job ended with error - connection string is empty");
                return;
            }

            await CleanAutomationNotificationsAsync(connectionString).ConfigureAwait(false);
            log.LogInformation($"End CleanAutomationNotifications function: {DateTime.Now}");
        }

        private static async Task CleanAutomationNotificationsAsync(string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand();
                command.CommandText = "[dbo].[notifications_CleanAutomationNotifications]";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter rowsUpdated = command.Parameters.AddWithValue("@rowsUpdated", SqlDbType.Int);
                rowsUpdated.Direction = ParameterDirection.ReturnValue;

                command.Connection = conn;

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                _log.LogInformation($"SUCCESS - CleanAutomationNotificationsAsync ended succesfully with {rowsUpdated.Value} rows deleted");
            }
            catch (Exception ex)
            {
                _log.LogError($"FAIL - CleanAutomationNotificationsAsync ended with fail: {ex.Message}");
            }
        }
    }
}
