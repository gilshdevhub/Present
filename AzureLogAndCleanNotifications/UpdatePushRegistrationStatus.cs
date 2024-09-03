using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureLogAndCleanNotifications
{
    public static class UpdatePushRegistrationStatus
    {
        private static ILogger _log;

        [FunctionName("UpdatePushRegistrationStatus")]
        // "TimerTrigger_UpdatePushRegistration": "0 0 2 * * *"
        public static async Task Run([TimerTrigger("%TimerTrigger_UpdatePushRegistration%")]TimerInfo myTimer, ILogger log)
        {
            _log = log;

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogError("job ended with error - connection string is empty");
                return;
            }

            await UpdatePushRegistrationAsync(connectionString).ConfigureAwait(false);
            log.LogInformation($"End UpdatePushRegistrationStatus function: {DateTime.Now}");
        }

        private static async Task UpdatePushRegistrationAsync(string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand();
                command.CommandText = "[dbo].[notifications_UpdatePushRegistration]";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter rowsUpdated = command.Parameters.AddWithValue("@rowsUpdated", SqlDbType.Int);
                rowsUpdated.Direction = ParameterDirection.ReturnValue;

                command.Connection = conn;

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                _log.LogInformation($"SUCCESS - UpdatePushRegistrationAsync ended succesfully with {rowsUpdated.Value} rows updated");
            }
            catch (Exception ex)
            {
                _log.LogError($"FAIL - UpdatePushRegistrationAsync ended with fail: {ex.Message}");
            }
        }
    }
}
