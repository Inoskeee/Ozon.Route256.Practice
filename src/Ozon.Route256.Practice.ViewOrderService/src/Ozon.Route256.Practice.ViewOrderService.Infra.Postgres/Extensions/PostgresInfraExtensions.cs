using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;
using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Connection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Connection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Repositories;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Services;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Builders;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Extensions;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Extensions;

public static class PostgresInfraExtensions
{
    private const string DbConnectionStringShard1 = "ROUTE256_VIEW_ORDER_SERVICE_DB_SHARD1_CONNECTION_STRINGS";
    private const string DbConnectionStringShard2 = "ROUTE256_VIEW_ORDER_SERVICE_DB_SHARD2_CONNECTION_STRINGS";

    public static void ConfigurePostgresInfrastructure(this IServiceCollection services)
    {
        string[] connectionStrings = GetConnectionStrings();
        
        services.AddMigration(connectionStrings);
        
        var config = DefaultConfigurationBuilder.Build(connectionStrings);
        
        services.AddSingleton<IDbStore>(new DbStore(config.Endpoints.ToArray()));
        services.AddSingleton<IShardingRule<long>>(new LongShardingRule(config.BucketsCount));
        services.AddScoped<IShardConnectionFactory, ShardConnectionFactory>();
        
        services.ConfigureRepositories();
        services.ConfigureMappers();

        services.AddScoped<IPostgresProvider, PostgresProvider>();
    }
    
    private static void AddMigration(this IServiceCollection services, string[] connectionStrings)
    {
        var config = DefaultConfigurationBuilder.Build(connectionStrings);
        ShardedMigrationRunner.MigrateUp(config.Endpoints);
    }
    
    private static void ConfigureRepositories(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.AddScoped<IOrdersRepository, OrdersRepository>();
    }
    
    private static void ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IOrderMapper, OrderMapper>();
    }

    private static string[] GetConnectionStrings()
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
}