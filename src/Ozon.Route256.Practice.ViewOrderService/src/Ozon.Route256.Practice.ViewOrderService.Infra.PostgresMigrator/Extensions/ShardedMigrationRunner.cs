using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations.Settings;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Extensions;

public static class ShardedMigrationRunner
{
    private static int _bucketsCount = 0;
    
    public static void MigrateUp(IReadOnlyList<DatabaseEndpoint> endpoints)
    {
        _bucketsCount = endpoints
            .SelectMany(x => x.Buckets)
            .Count();
        
        foreach (var endpoint in endpoints)
        {
            foreach (var bucketId in endpoint.Buckets)
            {
                var serviceProvider = CreateServices(
                    endpoint,
                    bucketId);
                using var scope = serviceProvider.CreateScope();

                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
    }
    
    private static IServiceProvider CreateServices(
        DatabaseEndpoint dbEndpoint,
        int bucketId)
    {
        var connectionString = dbEndpoint.ConnectionString;
        
        var services = new ServiceCollection();
        var provider = services
            .AddScoped<IBucketMigrationContext>(_ => new BucketMigrationContext(dbEndpoint, bucketId))
            .AddSingleton<IConventionSet>(new DefaultConventionSet(null, null))
            .AddSingleton<IShardingRule<long>>(new LongShardingRule(_bucketsCount))
            .AddFluentMigratorCore()
            .ConfigureRunner(builder => builder
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .WithRunnerConventions(new MigrationRunnerConventions())
                .WithMigrationsIn(typeof(ShardedMigrationRunner).Assembly)
                .ScanIn(typeof(ShardVersionTableMetaData).Assembly).For.VersionTableMetaData()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider();

        return provider;
    }
}