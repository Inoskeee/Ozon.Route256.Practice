using Mapster;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Mappers;

internal sealed class EntityMapper : IEntityMapper
{
    public OrderEntityWithCustomerEntity MapCustomerToRegionOrderEntity(OrderEntityWithCustomerId orderEntityWithCustomerId,
        CustomerEntity customerEntity)
    {
        var orderEntity = orderEntityWithCustomerId.Adapt<OrderEntityWithCustomerEntity>();
        orderEntity.Customer = customerEntity;
        return orderEntity;
    }
}