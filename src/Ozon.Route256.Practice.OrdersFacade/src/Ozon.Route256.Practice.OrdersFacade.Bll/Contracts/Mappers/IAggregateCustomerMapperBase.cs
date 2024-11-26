using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;

public interface IAggregateCustomerMapperBase<TMapTo>
{
    TMapTo MapByCustomerToResponse(AggregateByCustomerEntity aggregateByCustomerEntity);

}