using Mapster;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers;

internal sealed class HttpOrderMapper : IHttpOrderMapper
{
    public OrderEntityWithCustomerId MapToEntity(OrdersHttpResponseDto mapObject)
    {
        var orderEntity = mapObject.Adapt<OrderEntityWithCustomerId>();
        return orderEntity;
    }
}