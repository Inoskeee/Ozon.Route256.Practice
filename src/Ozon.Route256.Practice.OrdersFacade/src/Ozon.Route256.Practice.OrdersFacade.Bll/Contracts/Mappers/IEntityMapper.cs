using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;

public interface IEntityMapper
{
    OrderEntityWithCustomerEntity MapCustomerToRegionOrderEntity(
        OrderEntityWithCustomerId orderEntityWithCustomerId,
        CustomerEntity customerEntity);
}