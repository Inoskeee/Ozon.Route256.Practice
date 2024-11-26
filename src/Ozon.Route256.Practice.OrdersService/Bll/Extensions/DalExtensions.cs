using Ozon.Route256.OrderService.Bll.Models;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.Bll.Extensions;

public static class DalExtensions
{
    public static OrderEntity ToDal(this Order order)
    {
        return new OrderEntity
        {
            OrderId = order.OrderId,
            RegionId = order.Region.Id,
            CustomerId = order.CustomerId,
            Status = (int)order.Status,
            Comment = order.Comment,
            CreatedAt = order.CreatedAt
        };
    }

    public static Order ToBll(this OrderInfoEntity entity)
    {
        return new Order(
            OrderId: entity.OrderId,
            CustomerId: entity.CustomerId,
            Status: (OrderStatus)entity.Status,
            Region: new Region(
                Id: entity.RegionId,
                Name: entity.RegionName),
            Comment: entity.Comment,
            CreatedAt: entity.CreatedAt);
    }
    
    public static ItemEntity ToDal(this Item item, long orderId)
    {
        return new ItemEntity
        {
            OrderId = orderId,
            Barcode = item.Barcode,
            Quantity = item.Quantity
        };
    }
}