using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureLogAndCleanNotifications
{
    public static class CleanRailScheduals
    {
        private static ILogger _log;

        [FunctionName("CleanRailScheduals")]
        // "TimerTrigger_CleanRailScheduals": "0 15 2 * * *"
        public static async Task Run([TimerTrigger("%TimerTrigger_CleanRailScheduals%")]TimerInfo myTimer, ILogger log)
        {
            _log = log;

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogInformation("job ended with error - connection string is empty");
                return;
            }

            await CleanRailSchedualsAsync(connectionString).ConfigureAwait(false);
            log.LogInformation($"End CleanRailScheduals function: {DateTime.Now}");
        }

        private static async Task CleanRailSchedualsAsync(string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                using SqlCommand command = new SqlCommand();
                command.CommandText = "[dbo].[notifications_CleanRailScheduals]";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter rowsUpdated = command.Parameters.AddWithValue("@rowsUpdated", SqlDbType.Int);
                rowsUpdated.Direction = ParameterDirection.ReturnValue;

                command.Connection = conn;

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                _log.LogInformation($"SUCCESS - CleanRailSchedualsAsync ended succesfully with {rowsUpdated.Value} rows deleted");
            }
            catch (Exception ex)
            {
                _log.LogError($"FAIL - CleanRailSchedualsAsync ended with fail: {ex.Message}");
            }
        }
    }
}
