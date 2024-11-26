using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;

public interface IBucketMigrationContext
{
    public string CurrentDbSchema { get; }

    public int CurrentBucketId { get; }

    public DatabaseEndpoint CurrentEndpoint { get; }
}