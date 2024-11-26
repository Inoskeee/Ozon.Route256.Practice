namespace Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

public sealed class AggregateByRegionEntity
{
    public required RegionEntity Region { get; init; }
    
    public List<OrderEntityWithCustomerEntity> Orders { get; init; } = new List<OrderEntityWithCustomerEntity>();
}