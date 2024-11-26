using Ozon.Route256.Practice.ClientOrders.Bll.Models.Enums;
using Ozon.Route256.Practice.ClientOrders.Bll.Models.Order;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Contracts.Mappers;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Mappers;

internal sealed class OrderMapper : IOrderMapper
{
    public OrderModel MapOrderEntityToModel(OrderEntity orderEntity)
    {
        return new OrderModel(
            OrderId: orderEntity.OrderId,
            CustomerId: orderEntity.CustomerId,
            OrderStatus: (OrderStatusEnum)orderEntity.OrderStatus,
            CreatedAt: orderEntity.CreatedAt);
    }

    public OrderEntity MapOrderModelToEntity(OrderModel orderModel)
    {
        return new OrderEntity(
            OrderId: orderModel.OrderId,
            CustomerId: orderModel.CustomerId,
            OrderStatus: (int)orderModel.OrderStatus,
            CreatedAt: orderModel.CreatedAt.DateTime);
    }
}