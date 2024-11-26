using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Migrations.Database;

public static class MigrationManager
{
    public static IHost MigrateDatabase(
        this IHost host)
    {
        using var scope = host.Services.CreateScope();
        
        var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        try
        {
            migrationService.ListMigrations();
            migrationService.MigrateUp();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return host;
    }
}