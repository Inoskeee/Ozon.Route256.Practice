using Ozon.Route256.OrderService.Dal.Entities;

namespace Ozon.Route256.OrderService.Dal.Repositories;

public interface IItemsRepository
{
    Task Insert(
        ItemEntity[] items,
        CancellationToken token);
}