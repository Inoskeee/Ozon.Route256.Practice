using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Repositories;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Services;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Extensions;

public static class PostgresInfraExtensions
{
    private const string DbConnectionString = "ROUTE256_CLIENT_ORDER_SERVICE_DB_CONNECTION_STRING";

    public static IServiceCollection ConfigurePostgresInfrastructure(this IServiceCollection services)
    {
        string? connectionString = Environment.GetEnvironmentVariable(DbConnectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {DbConnectionString}. Заполните ее и перезапустите приложение");
        }
        
        services.AddMigration(connectionString);
        
        services.ConfigureRepositories();
        services.ConfigureMappers();

        services.AddScoped<IPostgresProvider, PostgresProvider>();
        
        return services;
    }
    
    private static IServiceCollection AddMigration(
        this IServiceCollection services,
        string connectionString)
    {
        return services
            .AddFluentMigratorCore()
            .ConfigureRunner(
                x => x.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly())
                    .For.Migrations());
    }
    
    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        services.AddScoped<IClientOrdersRepository, ClientOrdersRepository>();

        return services;
    }
    
    private static IServiceCollection ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IOrderMapper, OrderMapper>();

        return services;
    }
}