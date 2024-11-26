using Mapster;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers;

internal sealed class HttpCustomerMapper : IHttpCustomerMapper
{
    public CustomerEntity MapToEntity(CustomerDto mapObject)
    {
        var customerEntity = mapObject.Adapt<CustomerEntity>();
        return customerEntity;
    }
}