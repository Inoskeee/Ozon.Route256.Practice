using Mapster;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Mappers;

internal sealed class HttpAggregateResponseMapper : IHttpAggregateResponseMapper
{
    public AggregateByCustomerResponseDto MapByCustomerToResponse(AggregateByCustomerEntity aggregateByCustomerEntity)
    {
        var response = aggregateByCustomerEntity.Adapt<AggregateByCustomerResponseDto>();
        return response;
    }

    public AggregateByRegionResponseDto MapByRegionToResponse(AggregateByRegionEntity aggregateByRegionEntity)
    {
        var response = aggregateByRegionEntity.Adapt<AggregateByRegionResponseDto>();
        return response;
    }
}