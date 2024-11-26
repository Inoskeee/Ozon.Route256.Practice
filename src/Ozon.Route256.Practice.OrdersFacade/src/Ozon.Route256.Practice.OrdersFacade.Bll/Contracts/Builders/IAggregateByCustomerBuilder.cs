using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;

public interface IAggregateByCustomerBuilder
{ 
    AggregateByCustomerEntity Build(List<OrderEntityWithCustomerId> orders, List<CustomerEntity> customer);
}