using System.Data;
using Npgsql;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Connection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;
using Route256.SharderRepository.Infrastructure.Connection;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Repositories;

public class BaseShardRepository<TShardKey>
{
    protected const int DefaultTimeoutInSeconds = 5;

    private readonly IShardConnectionFactory _connectionFactory;
    private readonly IShardingRule<TShardKey> _shardingRule;

    public BaseShardRepository(
        IShardConnectionFactory connectionFactory,
        IShardingRule<TShardKey> shardingRule)
    {
        _connectionFactory = connectionFactory;
        _shardingRule = shardingRule;
    }
    
    protected async Task<ShardNpgsqlConnection> GetOpenedConnectionByShardKey(
        TShardKey shardKey,
        CancellationToken cancellationToken)
    {
        var bucketId = _shardingRule.GetBucketId(shardKey);
        var connection = await GetConnectionByBucketId(
            bucketId, 
            cancellationToken);

        return await OpenConnectionIfNeeded(
            connection, 
            cancellationToken);
    }
    
    protected async Task<ShardNpgsqlConnection> GetOpenedConnectionByBucket(
        int bucketId,
        CancellationToken cancellationToken)
    {
        var connection = await GetConnectionByBucketId(
            bucketId,
            cancellationToken);
        
        return await OpenConnectionIfNeeded(
            connection, 
            cancellationToken);
    }
    
    protected int GetBucketByShardKey(TShardKey shardKey) 
        => _shardingRule.GetBucketId(shardKey);

    protected IEnumerable<int> AllBuckets 
        => _connectionFactory.GetAllBuckets();

    private Task<ShardNpgsqlConnection> GetConnectionByBucketId(
        int bucketId, 
        CancellationToken cancellationToken)
    {
        var connectionString = _connectionFactory.GetConnectionString(bucketId);
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        MapCompositTypes(dataSourceBuilder);
        var dataSource = dataSourceBuilder.Build();
        
        var pgConnection = dataSource.CreateConnection();
        
        return Task.FromResult(new ShardNpgsqlConnection(pgConnection, bucketId));
    }

    private async Task <ShardNpgsqlConnection> OpenConnectionIfNeeded(
        ShardNpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        if (connection.State is ConnectionState.Closed)
        {
            await connection.OpenAsync(cancellationToken);
        }

        return connection;
    }
    
    private void MapCompositTypes(NpgsqlDataSourceBuilder builder)
    {
        builder.MapComposite<OrderEntity>("order_type");
    }
}