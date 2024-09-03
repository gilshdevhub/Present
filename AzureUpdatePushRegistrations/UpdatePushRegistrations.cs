using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureUpdatePushRegistrations
{
    public static class UpdatePushRegistrations
    {
        [FunctionName("UpdatePushRegistrations")]
        public static async Task Run([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                log.LogInformation("job ended with error - connection string is empty");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync().ConfigureAwait(false);

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "[dbo].[UpdatePushRegistration]";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter rowsUpdated = command.Parameters.AddWithValue("@rowsUpdated", SqlDbType.Int);
                    rowsUpdated.Direction = ParameterDirection.ReturnValue;

                    command.Connection = conn;

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    log.LogInformation($"{rowsUpdated.Value} rows were updated");
                }
            }
        }
    }
}
