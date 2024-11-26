using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Connection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Connection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Repositories;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Builders;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Extensions;
using Testcontainers.PostgreSql;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Fixtures;

    public class PostgreSqlFixture : IAsyncLifetime
    {
        private const string DbConnectionStringShard1 = "ROUTE256_VIEW_ORDER_SERVICE_DB_SHARD1_CONNECTION_STRINGS";
        private const string DbConnectionStringShard2 = "ROUTE256_VIEW_ORDER_SERVICE_DB_SHARD2_CONNECTION_STRINGS";
        private const string BucketsPerShard = "ROUTE256_VIEW_ORDER_SERVICE_DB_BUCKETS_PER_SHARD";
        
        public PostgreSqlContainer PostgreSqlContainerShard1 { get; private set; }
        public PostgreSqlContainer PostgreSqlContainerShard2 { get; private set; }
        
        public IOrdersRepository OrdersRepository { get; private set; }


        public async Task InitializeAsync()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            Environment.SetEnvironmentVariable(
                BucketsPerShard, "5");
            
            await SetConnectionStrings();

            string[] connectionStrings = GetConnectionStrings();
            
            var config = DefaultConfigurationBuilder.Build(connectionStrings);
            
            
            ShardedMigrationRunner.MigrateUp(config.Endpoints);
            
            var services = new ServiceCollection();
            
            var provider = services
                .AddSingleton<IDbStore>(new DbStore(config.Endpoints.ToArray()))
                .AddSingleton<IShardingRule<long>>(new LongShardingRule(config.BucketsCount))
                .AddScoped<IShardConnectionFactory, ShardConnectionFactory>()
                .AddScoped<IOrdersRepository, OrdersRepository>()
                .BuildServiceProvider();

            OrdersRepository = provider.GetRequiredService<IOrdersRepository>();
        
            FluentAssertionOptions.UseDefaultPrecision();
        }

        private async Task SetConnectionStrings()
        {
            PostgreSqlContainerShard1 = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("view-order-service-db-shard-1")
                .WithUsername("nvpopov")
                .WithPassword("pgpass")
                .Build();

            PostgreSqlContainerShard2 = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("view-order-service-db-shard-1")
                .WithUsername("nvpopov")
                .WithPassword("pgpass")
                .Build();
        
            await PostgreSqlContainerShard1.StartAsync();
            await PostgreSqlContainerShard1.StartAsync();
            
            var connectionStringShard1 = PostgreSqlContainerShard1.GetConnectionString();
            
            Environment.SetEnvironmentVariable(
                DbConnectionStringShard1, connectionStringShard1);
            
            var connectionStringShard2 = PostgreSqlContainerShard1.GetConnectionString();
            
            Environment.SetEnvironmentVariable(
                DbConnectionStringShard2, connectionStringShard2);
        }
    
        private string[] GetConnectionStrings()
        {
            string? connectionStringShard1 = Environment.GetEnvironmentVariable(DbConnectionStringShard1);

            if (string.IsNullOrWhiteSpace(connectionStringShard1))
            {
                throw new InvalidOperationException(
                    $"Отсутствует переменная окружения {DbConnectionStringShard1}. Заполните ее и перезапустите приложение");
            }
        
            string? connectionStringShard2 = Environment.GetEnvironmentVariable(DbConnectionStringShard2);

            if (string.IsNullOrWhiteSpace(connectionStringShard2))
            {
                throw new InvalidOperationException(
                    $"Отсутствует переменная окружения {DbConnectionStringShard2}. Заполните ее и перезапустите приложение");
            }

            return new[]
            {
                connectionStringShard1,
                connectionStringShard2
            };
        }
        
        
        public async Task DisposeAsync()
        {
            await PostgreSqlContainerShard1.StopAsync();
            await PostgreSqlContainerShard1.DisposeAsync();
            
            await PostgreSqlContainerShard2.StopAsync();
            await PostgreSqlContainerShard2.DisposeAsync();
        }
    }