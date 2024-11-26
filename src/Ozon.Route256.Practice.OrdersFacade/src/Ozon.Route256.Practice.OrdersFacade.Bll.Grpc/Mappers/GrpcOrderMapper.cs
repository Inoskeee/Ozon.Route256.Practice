using Mapster;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;
using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers;

internal sealed class GrpcOrderMapper : IGrpcOrderMapper
{
    public OrderEntityWithCustomerId MapToEntity(V1QueryOrdersResponse mapObject)
    {
        var orderEntity = mapObject.Adapt<OrderEntityWithCustomerId>();
        return orderEntity;
    }
}