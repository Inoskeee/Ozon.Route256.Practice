using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Mappers;

internal interface IOrderMapper
{
    OrderEntity MapModelToEntity(OrderModel orderModel);
    
    OrderModel MapEntityToModel(OrderEntity orderEntity);
}