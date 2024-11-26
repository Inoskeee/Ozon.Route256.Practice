using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.Bll.Models.Enums;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Mappers;

internal class OrderMapper : IOrderMapper
{
    public OrderEntity MapModelToEntity(OrderModel orderModel)
    {
        return new OrderEntity()
        {
            OrderId = orderModel.OrderId,
            CustomerId = orderModel.CustomerId,
            RegionId = orderModel.RegionId,
            Status = (int)orderModel.Status,
            Comment = orderModel.Comment,
            CreatedAt = orderModel.CreatedAt
        };
    }

    public OrderModel MapEntityToModel(OrderEntity orderEntity)
    {
        return new OrderModel()
        {
            OrderId = orderEntity.OrderId,
            CustomerId = orderEntity.CustomerId,
            RegionId = orderEntity.RegionId,
            Status = (OrderStatusEnum)orderEntity.Status,
            Comment = orderEntity.Comment,
            CreatedAt = orderEntity.CreatedAt
        };
    }
}