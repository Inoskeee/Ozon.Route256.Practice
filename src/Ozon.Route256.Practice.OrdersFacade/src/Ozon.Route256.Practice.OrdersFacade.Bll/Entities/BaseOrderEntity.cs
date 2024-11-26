using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

public abstract class BaseOrderEntity
{
    public long OrderId { get; init; }
    public required RegionEntity Region { get; init; }
    
    public OrderStatusEnum Status { get; init; }
    
    public string Comment { get; init; } = string.Empty;
    
    public DateTimeOffset CreatedAt { get; init; }
}