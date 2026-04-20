

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GabTrans.Domain.Constants
{
    public static class ConnectionString
    {
        public static string Get()
        {
            var host = Environment.GetEnvironmentVariable("DBHOST_GABTRANS");
            var port = Environment.GetEnvironmentVariable("DBPORT_GABTRANS");
            var username = Environment.GetEnvironmentVariable("DBUSER_GABTRANS");
            var password = Environment.GetEnvironmentVariable("DBPASSWORD_GABTRANS");
            var database = Environment.GetEnvironmentVariable("DBNAME_GABTRANS");

            return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true; Include Error Detail=true";
        }
    }
}
