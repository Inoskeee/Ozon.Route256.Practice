namespace Ozon.Route256.OrderService.Dal.Entities;

public class OrderLogEntity
{
    public long Id { get; init; }
    
    public long OrderId { get; init; }
    
    public long RegionId { get; init; }
    
    public long CustomerId { get; init; }
    
    public int Status { get; init; }

    public string Comment { get; init; } = string.Empty;

    public DateTimeOffset CreatedAt { get; init; }
    
    public DateTimeOffset UpdatedAt { get; init; }
}