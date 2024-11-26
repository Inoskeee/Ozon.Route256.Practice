using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Repositories;

internal interface IClientOrdersRepository
{
    Task<OrderEntity[]> GetByCustomerId(
        long customerId,
        CancellationToken cancellationToken);
    
    Task<OrderEntity[]> GetByOrderId(
        long orderId,
        CancellationToken cancellationToken);
    
    Task<ResultEntity<long?>> UpdateOrInsert(
        OrderEntity order,
        CancellationToken cancellationToken);

    Task<ResultEntity<int>> Delete(
        long orderId,
        CancellationToken cancellationToken);
}