using System;
using System.Collections.Generic;
using System.Text;

namespace AzureJobs
{
    public class Utils
    {
        protected Utils()
        {
        }

        public static string GetConnectionString()
        {
            string connectionString;

            string environment = Environment.GetEnvironmentVariable("Environment");

            if (string.Equals(environment, "production", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = Environment.GetEnvironmentVariable("sqldbprod_connection");
            }
            else
            {
                connectionString = Environment.GetEnvironmentVariable("sqldbqa_connection");
            }

            return connectionString;
        }
    }
}
