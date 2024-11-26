using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.OrderGrpc;

namespace Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts.Mappers;

public interface IOrderMapper
{
    OrderModel MapOrderDtoToModel(V1QueryOrdersResponse ordersResponse);
}