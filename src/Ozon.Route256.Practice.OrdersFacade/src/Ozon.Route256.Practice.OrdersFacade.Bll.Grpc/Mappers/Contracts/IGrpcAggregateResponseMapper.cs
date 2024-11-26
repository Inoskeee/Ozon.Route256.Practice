using Ozon.Route256.Practice.OrderFacade;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;

public interface IGrpcAggregateResponseMapper : 
    IAggregateCustomerMapperBase<GetOrderByCustomerResponse>, 
    IAggregateRegionMapperBase<GetOrderByRegionResponse>
{
    
}