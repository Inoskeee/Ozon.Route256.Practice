using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Mappers.Contracts;

public interface IHttpAggregateResponseMapper :
    IAggregateCustomerMapperBase<AggregateByCustomerResponseDto>, 
    IAggregateRegionMapperBase<AggregateByRegionResponseDto>
{
    
}