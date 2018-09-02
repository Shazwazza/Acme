using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Acme.Presentation.Website;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Acme.IntegrationTests
{
    [SetUpFixture]
    public class TestSuiteInitializer
    {

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            await DeleteAllTablesInDatabase(configuration.GetConnectionString("MsSql"));

        }

        private static async Task DeleteAllTablesInDatabase(string connectionString)
        {
            CreateIfNotExists(connectionString);

            var contextFactory = new DesignTimeDbContextFactory(connectionString);

            await contextFactory.CreateDbContext(Array.Empty<string>()).Database.EnsureDeletedAsync();
        }

        private static void CreateIfNotExists(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.InitialCatalog = "master";

            using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("select * from master.dbo.sysdatabases where name='{0}'", databaseName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // exists
                        {
                            return;
                        }
                    }

                    command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
        }
    }
}