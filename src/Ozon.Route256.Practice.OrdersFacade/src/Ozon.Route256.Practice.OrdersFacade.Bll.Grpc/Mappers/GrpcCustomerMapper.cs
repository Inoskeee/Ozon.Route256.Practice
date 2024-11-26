using Mapster;
using Ozon.Route256.Practice.CustomerService;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers;

internal sealed class GrpcCustomerMapper : IGrpcCustomerMapper
{
    public CustomerEntity MapToEntity(V1QueryCustomersResponse.Types.Customer mapObject)
    {
        var customerEntity = mapObject.Adapt<CustomerEntity>();
        return customerEntity;
    }
}