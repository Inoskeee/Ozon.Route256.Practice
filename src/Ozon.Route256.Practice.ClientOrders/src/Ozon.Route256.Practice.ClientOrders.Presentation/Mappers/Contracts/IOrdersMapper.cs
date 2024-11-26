using Google.Protobuf.Collections;
using Ozon.Route256.Practice.ClientOrders.Bll.Models;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;

namespace Ozon.Route256.Practice.ClientOrders.Presentation.Mappers.Contracts;

public interface IOrdersMapper
{
    RepeatedField<GetOrdersResponse.Types.Order> MapOrderModelToDto(OrderModel[] orders);
}