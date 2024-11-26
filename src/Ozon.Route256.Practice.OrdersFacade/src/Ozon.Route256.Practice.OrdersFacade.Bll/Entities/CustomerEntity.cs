namespace Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

public sealed class CustomerEntity
{
    public long CustomerId { get; init; }
    
    public required RegionEntity Region { get; init; }
    
    public string FullName { get; init; } = string.Empty;
    
    public DateTimeOffset CreatedAt { get; init; }
}