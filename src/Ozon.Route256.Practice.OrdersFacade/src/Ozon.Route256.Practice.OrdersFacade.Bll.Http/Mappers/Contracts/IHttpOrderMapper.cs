using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers.Contracts;

public interface IHttpOrderMapper : IOrderMapperBase<OrdersHttpResponseDto>
{
    
}