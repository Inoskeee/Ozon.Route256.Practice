using Mapster;
using Ozon.Route256.Practice.OrderFacade;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers;

internal sealed class GrpcAggregateResponseMapper : IGrpcAggregateResponseMapper
{
    public GetOrderByCustomerResponse MapByCustomerToResponse(AggregateByCustomerEntity aggregateByCustomerEntity)
    {
        GetOrderByCustomerResponse aggregateResponse = new GetOrderByCustomerResponse();
        
        aggregateResponse.Customer =
            aggregateByCustomerEntity.Customer.Adapt<GetOrderByCustomerResponse.Types.Customer>();
        
        foreach (var order in aggregateByCustomerEntity.Orders)
        {
            aggregateResponse.Orders.Add(order.Adapt<GetOrderByCustomerResponse.Types.CustomerOrders>());
        }
        
        return aggregateResponse;
    }

    public GetOrderByRegionResponse MapByRegionToResponse(AggregateByRegionEntity aggregateByRegionEntity)
    {
        GetOrderByRegionResponse aggregateResponse = new GetOrderByRegionResponse();
        
        aggregateResponse.Region =
            aggregateByRegionEntity.Region.Adapt<GetOrderByRegionResponse.Types.Region>();
        
        foreach (var order in aggregateByRegionEntity.Orders)
        {
            aggregateResponse.Orders.Add(order.Adapt<GetOrderByRegionResponse.Types.RegionOrders>());
        }
        
        return aggregateResponse;
    }
}