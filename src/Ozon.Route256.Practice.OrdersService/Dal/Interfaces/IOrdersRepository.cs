using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Models;

namespace Ozon.Route256.OrderService.Dal.Interfaces;

public interface IOrdersRepository
{
    Task<long[]> Insert(
        OrderEntity order,
        CancellationToken token);

    Task Update(
        OrderEntity order,
        CancellationToken token);

    Task<OrderInfoEntity[]> Query(
        OrderQueryModel query,
        CancellationToken token);
}