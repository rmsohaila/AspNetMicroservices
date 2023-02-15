using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Threading;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost InitMigration<T>(this IHost host, int? retryValue = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();
                var connectionString = config.GetValue<string>("ConnectionStrings:PGConnectionString");

                try
                {
                    logger.LogInformation("Migrating postgresql database started");

                    MigrateSchema(connectionString);

                    logger.LogInformation("Migration completed.");

                    logger.LogInformation("Seeding database records, started");

                    SeedData(connectionString);

                    logger.LogInformation("Database seeding completed.");
                }
                catch (NpgsqlException ex)
                {
                    retryValue++;
                    logger.LogError(ex, $"An error occured while migrating postgresql database, retrying attemp ({retryValue})");
                    Thread.Sleep(2000);
                    InitMigration<T>(host, retryValue);
                }

                return host;
            }
        }

        private static void MigrateSchema(string connectionString)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand { Connection = conn };

            cmd.CommandText = "DROP TABLE IF EXISTS Coupons";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE Coupons (
                                    Id serial PRIMARY KEY,
                                    ProductName VARCHAR(50),
                                    Description TEXT,
                                    Amount INT
                                )";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private static void SeedData(string connectionString)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand { Connection = conn };

            cmd.CommandText = @"INSERT INTO Coupons(ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone X Discount', 500)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"INSERT INTO Coupons(ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung 10 Discount', 300)";
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
