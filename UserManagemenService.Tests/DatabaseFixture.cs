using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using Xunit;
using UserManagemenService.Models;

namespace UserManagemenService.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly string _connectionString;
        public readonly DbContextOptions<UserManagemenServiceContext> dbContextOptions;
        private readonly Respawner _respawner;

        public DatabaseFixture()
        {
            _connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING") ?? throw new ArgumentNullException("ASPNETCORE_CONNECTION_STRING is null");

            dbContextOptions = new DbContextOptionsBuilder<UserManagemenServiceContext>()
                .UseNpgsql(_connectionString)
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .Options;
            using (var dbContext = new UserManagemenServiceContext(this.dbContextOptions))
            {
                Console.WriteLine("");
                Console.WriteLine("Creating testing database");
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
            }

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                // var result = task.WaitAndUnwrapException();
                var task = Respawner.CreateAsync(conn, new RespawnerOptions
                {
                    SchemasToInclude = new []
                    {
                        "public"
                    },
                    DbAdapter = DbAdapter.Postgres
                });
                task.Wait();
                _respawner = task.Result;
            }
        }

        public string ConnectionString()
        {
            return _connectionString;
        }

        public async Task ResetDatabase()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                await _respawner.ResetAsync(conn);
            }
        }

        public void Dispose()
        {
            using (var dbContext = new UserManagemenServiceContext(this.dbContextOptions))
            {
                Console.WriteLine("");
                Console.WriteLine("Deleting testing database");
                dbContext.Database.EnsureDeleted();
            }
        }
    }
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    { }
}