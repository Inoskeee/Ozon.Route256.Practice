namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;

public sealed record OrderEntity
{
    public required long OrderId { get; init; }
    
    public required long RegionId { get; init; }
    
    public required int Status { get; init; }
    
    public required long CustomerId { get; init; }
    
    public required string Comment { get; init; }
    
    public required DateTime CreatedAt { get; init; }
}