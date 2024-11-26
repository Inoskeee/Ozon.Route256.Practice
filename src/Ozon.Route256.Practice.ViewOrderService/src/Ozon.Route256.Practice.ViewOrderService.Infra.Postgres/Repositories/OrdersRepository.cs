using Dapper;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Connection;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresCommon.Rules;
using Ozon.Route256.Practice.ViewOrderService.Infra.PostgresMigrator.Contracts;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Repositories;

public sealed class OrdersRepository(
    IShardConnectionFactory connectionFactory,
    IShardingRule<long> shardingRule)
    : BaseShardRepository<long>(connectionFactory, shardingRule), 
        IOrdersRepository
{
    
    public async Task AddOrder(OrderEntity order, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
                           insert into __bucket__.orders (order_id, region_id, status, customer_id, comment, created_at)
                            select order_id,
                                   region_id,
                                   status,
                                   customer_id,
                                   comment,
                                   created_at
                             from unnest(@Orders)
                           """;

        var cmd = new CommandDefinition(
            sql,
            new
            {
                Orders = new [] { order }
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);
        
        await using var connection = await GetOpenedConnectionByShardKey(
            order.OrderId, 
            cancellationToken); 
        
        await connection.ExecuteAsync(cmd);
    }

    public async Task UpdateOrder(OrderEntity order, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
                           update __bucket__.orders
                              set region_id = @RegionId,
                                  status = @Status,
                                  customer_id = @CustomerId,
                                  comment = @Comment,
                                  created_at = @CreatedAt
                            where order_id = @OrderId
                           """;

        var cmd = new CommandDefinition(
            sql,
            new
            {
                OrderId = order.OrderId,
                RegionId = order.RegionId,
                Status = order.Status,
                CustomerId = order.CustomerId,
                Comment = order.Comment,
                CreatedAt = order.CreatedAt
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);
    
        await using var connection = await GetOpenedConnectionByShardKey(
            order.OrderId, 
            cancellationToken);

        await connection.ExecuteAsync(cmd);
    }

    public async Task<OrderEntity?> GetOrderById(long orderId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = """
                           select * from __bucket__.orders
                               where order_id = @OrderId
                           """;

        var cmd = new CommandDefinition(
            sql,
            new
            {
                OrderId = orderId
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);
        
        await using var connection = await GetOpenedConnectionByShardKey(
            orderId, 
            cancellationToken);
        
        return await connection.QuerySingleOrDefaultAsync<OrderEntity>(cmd);
    }

    public async Task<IReadOnlyList<OrderEntity>> GetOrdersByOrderId(long[] orderIds, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (orderIds.Length == 0)
        {
            throw new ArgumentException("The array of orderIds is empty");
        }
        
        var sql = """
                  select * from __bucket__.orders
                           where 1 = 1
                  """;
        
        if (orderIds.Any())
        {
            sql += " and order_id = any(@OrderIds)";
        }
        
        var cmd = new CommandDefinition(
            sql,
            new
            {
                OrderIds = orderIds
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);

        var result = new List<OrderEntity>();
        
        foreach (var bucket in AllBuckets)
        {
            await using var connection = await GetOpenedConnectionByBucket(
                bucket, 
                cancellationToken);

            var studentsInBucket = await connection
                .QueryAsync<OrderEntity>(cmd);
            
            result.AddRange(studentsInBucket);
        }

        return result;
    }

    public async Task<IReadOnlyList<OrderEntity>> GetOrdersByRegionId(long[] regionIds, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (regionIds.Length == 0)
        {
            throw new ArgumentException("The array of regionIds is empty");
        }
    
        var sql = """
                  select * from __bucket__.orders
                           where region_id = any(@RegionIds)
                  """;
    
        var cmd = new CommandDefinition(
            sql,
            new
            {
                RegionIds = regionIds
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);

        var result = new List<OrderEntity>();

        foreach (var bucket in AllBuckets)
        {
            await using var connection = await GetOpenedConnectionByBucket(
                bucket, 
                cancellationToken);

            var ordersInBucket = await connection
                .QueryAsync<OrderEntity>(cmd);
        
            result.AddRange(ordersInBucket);
        }

        return result;
    }

    public async Task<IReadOnlyList<OrderEntity>> GetOrdersByCustomerId(long[] customerIds, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (customerIds.Length == 0)
        {
            throw new ArgumentException("The array of customerIds is empty");
        }
    
        var sql = """
                  select * from __bucket__.orders
                           where customer_id = any(@CustomerIds)
                  """;
    
        var cmd = new CommandDefinition(
            sql,
            new
            {
                CustomerIds = customerIds
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);

        var result = new List<OrderEntity>();

        foreach (var bucket in AllBuckets)
        {
            await using var connection = await GetOpenedConnectionByBucket(
                bucket, 
                cancellationToken);

            var ordersInBucket = await connection
                .QueryAsync<OrderEntity>(cmd);
        
            result.AddRange(ordersInBucket);
        }

        return result;
    }

public async Task<IReadOnlyList<OrderEntity>> GetOrdersByLimit(
    long limit, long offset, CancellationToken cancellationToken)
{
    cancellationToken.ThrowIfCancellationRequested();

    if (limit <= 0)
    {
        throw new ArgumentException("Limit should be greater than 0");
    }

    var result = new List<OrderEntity>();
    long currentOffset = offset;
    long currentLimit = limit;
    
    var recordsCount = await GetRecordsForAllBucket(cancellationToken);

    var orderedBuckets = recordsCount.Keys.OrderBy(b => b);
    
    foreach (var bucket in orderedBuckets)
    {
        cancellationToken.ThrowIfCancellationRequested();

        long totalRecordsInBucket = recordsCount[bucket];

        if (currentOffset >= totalRecordsInBucket)
        {
            currentOffset -= totalRecordsInBucket;
            continue;
        }
        
        long bucketOffset = currentOffset;
        long bucketLimit = Math.Min(currentLimit, totalRecordsInBucket - bucketOffset);

        var sql = """
            select * from __bucket__.orders
            order by order_id desc
            limit @Limit offset @Offset
            """;

        var cmd = new CommandDefinition(
            sql,
            new
            {
                Limit = bucketLimit,
                Offset = bucketOffset
            },
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);

        await using var connection = await GetOpenedConnectionByBucket(bucket, cancellationToken);

        var ordersInBucket = await connection.QueryAsync<OrderEntity>(cmd);

        result.AddRange(ordersInBucket);

        currentLimit -= ordersInBucket.Count();
        currentOffset = 0;

        if (currentLimit <= 0)
        {
            break;
        }
    }

    return result;
}

    private async Task<Dictionary<int, long>> GetRecordsForAllBucket(CancellationToken cancellationToken)
    {
        var bucketCounts = new Dictionary<int, long>();

        foreach (var bucket in AllBuckets)
        {
            cancellationToken.ThrowIfCancellationRequested();
            bucketCounts[bucket] = await GetTotalRecordsInBucket(bucket, cancellationToken);
        }

        return bucketCounts;
    }

    private async Task<long> GetTotalRecordsInBucket(int bucketId, CancellationToken cancellationToken)
    {
        var sql = """
            select count(*) from __bucket__.orders
            """;

        var cmd = new CommandDefinition(
            sql,
            commandTimeout: DefaultTimeoutInSeconds,
            cancellationToken: cancellationToken);

        await using var connection = await GetOpenedConnectionByBucket(bucketId, cancellationToken);

        return await connection.ExecuteScalarAsync<long>(cmd);
    }
}