using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Repositories;

public interface IOrdersRepository
{
    Task AddOrder(OrderEntity order, CancellationToken cancellationToken);
    
    Task UpdateOrder(OrderEntity order, CancellationToken cancellationToken);
    
    Task<OrderEntity?> GetOrderById(long orderId, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<OrderEntity>> GetOrdersByOrderId(long[] orderIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<OrderEntity>> GetOrdersByRegionId(long[] regionIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<OrderEntity>> GetOrdersByCustomerId(long[] customerIds, CancellationToken cancellationToken);
    
    Task<IReadOnlyList<OrderEntity>> GetOrdersByLimit(long limit, long offset, CancellationToken cancellationToken);
}