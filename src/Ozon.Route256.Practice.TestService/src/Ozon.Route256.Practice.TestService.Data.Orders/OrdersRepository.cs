using Ozon.Route256.TestService.Common.Data;

namespace Ozon.Route256.TestService.Data.Orders;

public class OrdersRepository : IOrdersRepository
{
    private readonly IDbFacade _dbFacade;

    public OrdersRepository(IDbFacade<OrdersDataConnectionOptions> dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public async Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken)
    {
        var query = OrdersSqlHelper.CreateGetOrderByIdCommand(orderId, cancellationToken);

        var result = await _dbFacade.QueryAsync<Order>(query);

        var order = result.FirstOrDefault();

        if (order == null)
            return null;

        query = OrdersSqlHelper.CreateSelectItemsByOrderIdCommand(orderId, cancellationToken);

        var items = await _dbFacade.QueryAsync<OrderItem>(query);

        order.Items = items.ToArray();

        return order;
    }

    public async Task<IReadOnlyCollection<OrderItem>> SelectItemsByOrderIdAsync(long orderId, CancellationToken cancellationToken)
    {
        var query = OrdersSqlHelper.CreateSelectItemsByOrderIdCommand(orderId, cancellationToken);

        var result = await _dbFacade.QueryAsync<OrderItem>(query);

        return result.ToArray();
    }

    public async Task<IReadOnlyCollection<Order>> SearchOrdersAsync(SearchOrdersQueryParams queryParams, CancellationToken cancellationToken)
    {
        var query = OrdersSqlHelper.CreateSearchOrdersCommand(queryParams, cancellationToken);

        var result = await _dbFacade.QueryAsync<Order>(query);

        var orders = result.ToArray();

        if (orders.Length == 0)
            return orders;

        var orderIds = orders
            .Select(order => order.OrderId)
            .ToArray();

        query = OrdersSqlHelper.CreateSelectItemsByOrderIdCommand(orderIds, cancellationToken);

        var orderItemsMap = (await _dbFacade.QueryAsync<OrderItem>(query))
            .GroupBy(item => item.OrderId)
            .ToDictionary(group => group.Key, group => group.ToArray());

        foreach (var order in orders)
        {
            if (orderItemsMap.TryGetValue(order.OrderId, out var items))
            {
                order.Items = items;
            }
        }

        return orders;
    }
}
