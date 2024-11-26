using Dapper;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Interfaces;
using Ozon.Route256.OrderService.Dal.Models;

namespace Ozon.Route256.OrderService.Dal.Repositories;

public class OrdersRepository : BaseRepository, IOrdersRepository
{
    private const string OrdersTableFields = "region_id, status, customer_id, comment, created_at";
    
    public OrdersRepository(IConfiguration configuration) : base(configuration)
    {
    }
    
    public async Task<long[]> Insert(
        OrderEntity order,
        CancellationToken token)
    {
        const string sqlQuery = @$"
                insert into 
                    orders ({OrdersTableFields})
                values 
                    (@RegionId, @Status, @CustomerId, @Comment, @CreatedAt)
                returning order_id;";

        var param = new DynamicParameters();
        param.Add("RegionId", order.RegionId);
        param.Add("Status", order.Status);
        param.Add("CustomerId", order.CustomerId);
        param.Add("Comment", order.Comment);
        param.Add("CreatedAt", order.CreatedAt);
        
        return await ExecuteQueryAsync<long>(sqlQuery, param, token);
    }

    public async Task Update(
        OrderEntity order,
        CancellationToken token)
    {
        const string sqlQuery = $@"
                update
                    orders 
                set 
                    region_id = @RegionId,
                    status = @Status,
                    customer_id = @CustomerId,
                    comment = @Comment
                where 
                    order_id = @OrderId;";

        var param = new DynamicParameters();
        param.Add("OrderId", order.OrderId);
        param.Add("RegionId", order.RegionId);
        param.Add("Status", order.Status);
        param.Add("CustomerId", order.CustomerId);
        param.Add("Comment", order.Comment);
        
        await ExecuteNonQueryAsync(sqlQuery, param, token);
    }

    public async Task<OrderInfoEntity[]> Query(
        OrderQueryModel query,
        CancellationToken token)
    {
        if (query.IsEmpty())
        {
            return Array.Empty<OrderInfoEntity>();
        }

        var sqlQuery = @$"
                select 
                    order_id,
                    status,
                    customer_id,
                    r.id as region_id,
                    r.name as region_name,
                    created_at,
                    comment,
                    count(*) OVER() as total_count
                from 
                    orders
                join 
                    regions r on orders.region_id = r.id";
        
        var conditions = new List<string>();
        var param = new DynamicParameters();

        if (query.OrderIds is { Length: > 0 })
        {
            conditions.Add("order_id = any(@OrderIds)");
            param.Add("OrderIds", query.OrderIds);
        }
        
        if (query.CustomerIds is { Length: > 0 })
        {
            conditions.Add("customer_id = any(@CustomerIds)");
            param.Add("CustomerIds", query.CustomerIds);
        }
        
        if (query.RegionIds is { Length: > 0 })
        {
            conditions.Add("region_id = any(@RegionIds)");
            param.Add("RegionIds", query.RegionIds);
        }
        
        sqlQuery += " where " + string.Join(" and ", conditions);
        
        if (query.Limit > 0)
        {
            sqlQuery += " limit @Limit ";
            param.Add("Limit", query.Limit);
        }
        
        if (query.Offset > 0)
        {
            sqlQuery += " offset @Offset ";
            param.Add("Offset", query.Offset);
        }
        
        return await ExecuteQueryAsync<OrderInfoEntity>(sqlQuery, param, token);
    }
}