using Ozon.Route256.Practice.ClientOrders.Bll.Models.Enums;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.OrderGrpc;

namespace Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Mappers;

internal sealed class OrderMapper : IOrderMapper
{
    public OrderModel MapOrderDtoToModel(V1QueryOrdersResponse ordersResponse)
    {
        return new OrderModel(
            OrderId: ordersResponse.OrderId,
            CustomerId: ordersResponse.CustomerId,
            OrderStatus: (OrderStatusEnum)ordersResponse.Status,
            CreatedAt: ordersResponse.CreatedAt.ToDateTimeOffset()
        );
    }
}