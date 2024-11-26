using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts;

public interface IOrderFacadeHttpService
{
    Task<AggregateByCustomerEntity> GetOrderByCustomer(long customerId, int limit, int offset);
    
    Task<AggregateByRegionEntity> GetOrderByRegion(long regionId, int limit, int offset);
}