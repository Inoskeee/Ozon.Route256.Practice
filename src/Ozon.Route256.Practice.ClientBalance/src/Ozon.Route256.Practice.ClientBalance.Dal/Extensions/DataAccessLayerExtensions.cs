using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;
using Ozon.Route256.Practice.ClientBalance.Dal.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Services;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Extensions;

public static class DataAccessLayerExtensions
{
    private const string DbConnectionString = "ROUTE256_VIEW_ORDER_SERVICE_DB_CONNECTION_STRINGS";

    public static IServiceCollection ConfigureDalServices(this IServiceCollection services)
    {
        string? connectionString = Environment.GetEnvironmentVariable(DbConnectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Отсутствует переменная окружения {DbConnectionString}. Заполните ее и перезапустите приложение");
        }

        services.AddMigration(connectionString);
        services.AddRepositories();
        
        return services;
    }
    
    private static IServiceCollection AddMigration(this IServiceCollection services,
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
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IClientBalanceRepository, ClientBalanceRepository>();
        services.AddScoped<IOperationRepository<TopUpOperationEntity>, TopUpOperationRespository>();
        services.AddScoped<IOperationRepository<WithdrawOperationEntity>, WithdrawOperationRepository>();

        services.AddScoped<IClientBalanceUnitOfWork, ClientBalanceUnitOfWork>();
        services.AddScoped<IClientDbProvider, ClientDbProvider>();
        services.AddScoped<IBalanceOperationsDbProvider, BalanceOperationsDbProvider>();
        
        return services;
    }
}