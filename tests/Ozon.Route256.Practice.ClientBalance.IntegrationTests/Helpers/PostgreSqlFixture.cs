using Ozon.Route256.OrderService.Migrations;

namespace Ozon.Route256.Practice.ClientBalance.IntegrationTests.Helpers;

    public class PostgreSqlFixture : IAsyncLifetime
    {
        public PostgreSqlContainer? PostgreSqlContainer { get; private set; }

        public async Task InitializeAsync()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            PostgreSqlContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("client-balance-db")
                .WithUsername("nvpopov")
                .WithPassword("pgpass")
                .Build();

            await PostgreSqlContainer.StartAsync();
            
            var connectionString = PostgreSqlContainer.GetConnectionString();
            
            Environment.SetEnvironmentVariable(
                "ROUTE256_VIEW_ORDER_SERVICE_DB_CONNECTION_STRINGS", connectionString);

            RunMigrations(connectionString);
        }

        private void RunMigrations(string connectionString)
        {
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(CreateClientsTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
            
            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        public async Task DisposeAsync()
        {
            await PostgreSqlContainer.StopAsync();
            await PostgreSqlContainer.DisposeAsync();
        }
    }