using Ozon.Route256.Practice.ViewOrderService.Bll.Models.Enums;

namespace Ozon.Route256.Practice.ViewOrderService.Bll.Models;

public sealed record OrderModel
{
    public required long OrderId { get; init; }
    
    public required long RegionId { get; init; }
    
    public required OrderStatusEnum Status { get; init; }
    
    public required long CustomerId { get; init; }
    
    public required string Comment { get; init; }
    
    public required DateTime CreatedAt { get; init; }
}