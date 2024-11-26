using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;

namespace Ozon.Route256.Practice.ClientOrders.Bll.Contracts.ExternalContracts;

public interface IOrdersServiceProvider
{
    Task<OrderModel?> GetOrder(long orderId, string comment);
}