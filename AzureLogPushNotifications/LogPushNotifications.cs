using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureLogPushNotifications
{
    public static class LogPushNotifications
    {
        [FunctionName("LogPushNotifications")]
        public static async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer, ILogger log)
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
                    command.CommandText = "[dbo].[LogPushNotifications]";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter tranCount = command.Parameters.AddWithValue("@tranCount", SqlDbType.Int);
                    tranCount.Direction = ParameterDirection.ReturnValue;

                    command.Connection = conn;

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    log.LogInformation((int)tranCount.Value == 0 ? "job ended succesfully" : "job ended with error");
                }
            }
        }
    }
}
