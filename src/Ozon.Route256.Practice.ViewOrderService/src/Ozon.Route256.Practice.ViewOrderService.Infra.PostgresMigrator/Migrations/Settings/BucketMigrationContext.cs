using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Configuration;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Helpers;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Extensions;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Migrations.Settings;

public sealed class BucketMigrationContext : IBucketMigrationContext
{
    private readonly DatabaseEndpoint _currentShard;
    
    private string _currentDbSchema = string.Empty;
    private int _currentBucketId = 0;


    public BucketMigrationContext(
        DatabaseEndpoint shardEndpoint,
        int bucketId)
    {
        _currentShard = shardEndpoint;
        _currentBucketId = bucketId;
        _currentDbSchema = ShardsHelper.GetSchemaName(bucketId);
    }

    public string CurrentDbSchema => _currentDbSchema;
    
    public int CurrentBucketId => _currentBucketId;

    public DatabaseEndpoint CurrentEndpoint => _currentShard;
}