using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;

public interface IGrpcOrderMapper : IOrderMapperBase<V1QueryOrdersResponse>
{
    
}