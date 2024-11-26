using Ozon.Route256.Practice.ViewOrderService.Bll.Models;

namespace Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;

public interface IOrdersServiceProvider
{
    Task<OrderModel?> GetOrder(long orderId);
}