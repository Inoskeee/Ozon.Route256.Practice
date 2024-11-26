namespace Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

public sealed class AggregateByCustomerEntity
{
    public CustomerEntity? Customer { get; init; }
    public List<OrderEntityWithCustomerId> Orders { get; init; } = new List<OrderEntityWithCustomerId>();
}