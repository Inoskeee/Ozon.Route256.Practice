using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Builders;

public sealed class AggregateByCustomerBuilder : IAggregateByCustomerBuilder
{
    public AggregateByCustomerEntity Build(List<OrderEntityWithCustomerId> orders, List<CustomerEntity> customer)
    {
        AggregateByCustomerEntity aggregateByCustomerEntity = new AggregateByCustomerEntity()
        {
            Customer = customer.FirstOrDefault(),
            Orders = orders
        };

        return aggregateByCustomerEntity;
    }
}