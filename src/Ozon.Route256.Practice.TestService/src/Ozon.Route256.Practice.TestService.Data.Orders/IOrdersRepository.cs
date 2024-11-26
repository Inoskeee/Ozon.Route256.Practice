namespace Ozon.Route256.TestService.Data.Orders;

public interface IOrdersRepository
{
    Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<OrderItem>> SelectItemsByOrderIdAsync(long orderId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Order>> SearchOrdersAsync(SearchOrdersQueryParams queryParams, CancellationToken cancellationToken);
}
