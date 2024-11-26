using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Mappers;

internal interface IOrderMapper
{
    OrderModel MapOrderEntityToModel(OrderEntity orderEntity);
    
    OrderEntity MapOrderModelToEntity(OrderModel orderModel);
}