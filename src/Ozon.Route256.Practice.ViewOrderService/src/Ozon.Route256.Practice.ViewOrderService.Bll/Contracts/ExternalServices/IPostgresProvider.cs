using Ozon.Route256.Practice.ViewOrderService.Bll.Models;

namespace Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;

public interface IPostgresProvider
{
    Task UpdateOrInsertOrder(OrderModel order, CancellationToken cancellationToken);
}