using Ozon.Route256.Practice.CustomerService;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;

public interface IGrpcCustomerMapper : ICustomerMapperBase<V1QueryCustomersResponse.Types.Customer>
{
    
}