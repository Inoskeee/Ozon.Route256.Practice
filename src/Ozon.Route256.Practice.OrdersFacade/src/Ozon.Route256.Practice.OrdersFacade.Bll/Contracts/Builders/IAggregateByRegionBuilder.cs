using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;

public interface IAggregateByRegionBuilder
{
    AggregateByRegionEntity Build(List<OrderEntityWithCustomerId> orderEntities, List<CustomerEntity> customerEntities);
}