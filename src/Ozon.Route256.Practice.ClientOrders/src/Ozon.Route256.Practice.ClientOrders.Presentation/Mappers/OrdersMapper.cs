using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientOrders.Presentation.Mappers;

internal sealed class OrdersMapper : IOrdersMapper
{
    public RepeatedField<GetOrdersResponse.Types.Order> MapOrderModelToDto(OrderModel[] orders)
    {
        var ordersList = new RepeatedField<GetOrdersResponse.Types.Order>();

        foreach (var order in orders)
        {
            var orderDto = new GetOrdersResponse.Types.Order()
            {
                OrderId = order.OrderId,
                Status = (OrderStatus)order.OrderStatus,
                CreatedAt = order.CreatedAt.ToTimestamp()
            };
            ordersList.Add(orderDto);
        }

        return ordersList;
    }
}