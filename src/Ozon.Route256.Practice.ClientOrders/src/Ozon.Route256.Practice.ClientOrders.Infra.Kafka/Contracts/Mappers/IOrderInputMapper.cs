using Ozon.Route256.Practice.ClientOrders.Bll.Models;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Contracts.Mappers;

internal interface IOrderInputMapper
{
    OrderInputMessage MapOrderModelToMessage(OrderInputModel orderInputModel);
}