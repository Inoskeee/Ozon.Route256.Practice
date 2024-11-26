using Dapper;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Repositories;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Repositories;

internal sealed class ClientOrdersRepository : BaseRepository, IClientOrdersRepository
{
    public async Task<OrderEntity[]> GetByCustomerId(long customerId, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                                select *
                                from client_orders
                                where customer_id = @CustomerId
                                """;

        var param = new DynamicParameters();
        param.Add("CustomerId", customerId);
        
        return await ExecuteQueryAsync<OrderEntity>(sqlQuery, param, cancellationToken); 
    }

    public async Task<OrderEntity[]> GetByOrderId(long orderId, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                                select *
                                from client_orders
                                where order_id = @OrderId
                                """;

        var param = new DynamicParameters();
        param.Add("OrderId", orderId);

        return await ExecuteQueryAsync<OrderEntity>(sqlQuery, param, cancellationToken);
    }

    public async Task<ResultEntity<long?>> UpdateOrInsert(OrderEntity order, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                                insert into client_orders (order_id, customer_id, order_status, created_at)
                                values (@OrderId, @CustomerId, @OrderStatus, @CreatedAt)
                                on conflict (order_id)
                                do update set
                                    order_status = @OrderStatus,
                                    created_at = @CreatedAt
                                returning order_id;
                                """;

        var param = new DynamicParameters();
        param.Add("OrderId", order.OrderId);
        param.Add("CustomerId", order.CustomerId);
        param.Add("OrderStatus", order.OrderStatus);
        param.Add("CreatedAt", order.CreatedAt);

        var orderId = await ExecuteNonQueryAsync<long?>(sqlQuery, param, cancellationToken);
        
        if(orderId is not null)
        {
            return new ResultEntity<long?>(
                IsSuccess: true, 
                Result: orderId, 
                Message: "Заказ успешно сохранен");
        }
        else
        {
            return new ResultEntity<long?>(
                IsSuccess: false, 
                Result: null, 
                Message: "Не удалось сохранить заказ в БД");
        }
    }

    public async Task<ResultEntity<int>> Delete(long orderId, CancellationToken cancellationToken)
    {
        const string sqlQuery = """
                                delete from client_orders
                                where order_id = @OrderId
                                """;

        var param = new DynamicParameters();
        param.Add("OrderId", orderId);

        var result = await ExecuteNonQueryAsync<int>(sqlQuery, param, cancellationToken);
        
        if(result > 0)
        {
            return new ResultEntity<int>(
                IsSuccess: true, 
                Result: result, 
                Message: "Заказ успешно удален");
        }
        else
        {
            return new ResultEntity<int>(
                IsSuccess: false, 
                Result: result, 
                Message: "Не удалось удалить заказ из БД");
        }
    }
}