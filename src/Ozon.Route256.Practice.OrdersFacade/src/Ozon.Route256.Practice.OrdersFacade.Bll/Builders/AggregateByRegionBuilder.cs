using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Mappers;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Builders;

public sealed class AggregateByRegionBuilder : IAggregateByRegionBuilder
{
    private readonly IEntityMapper _entityMapper;

    public AggregateByRegionBuilder(IEntityMapper entityMapper)
    {
        _entityMapper = entityMapper;
    }

    public AggregateByRegionEntity Build(List<OrderEntityWithCustomerId> orderEntities, List<CustomerEntity> customerEntities)
    {
        AggregateByRegionEntity aggregateByRegionEntity = new AggregateByRegionEntity()
        {
            Region = orderEntities.First().Region
        };

        var customerDictionary = customerEntities.ToDictionary(key => key.CustomerId);
        
        List<OrderEntityWithCustomerEntity> ordersByRegionList = new List<OrderEntityWithCustomerEntity>();
        foreach (var order in orderEntities)
        {
            if (customerDictionary.TryGetValue(order.CustomerId, out var customer))
            {
                var byRegionOrder = _entityMapper.MapCustomerToRegionOrderEntity(order, customer);
                ordersByRegionList.Add(byRegionOrder);
            }
        }

        aggregateByRegionEntity.Orders.AddRange(ordersByRegionList);
        
        return aggregateByRegionEntity;
    }
}