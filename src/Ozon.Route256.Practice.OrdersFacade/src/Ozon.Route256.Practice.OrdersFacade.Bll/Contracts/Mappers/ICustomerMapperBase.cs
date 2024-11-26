using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;

public interface ICustomerMapperBase<TMapFrom>
{ 
    CustomerEntity MapToEntity(TMapFrom mapObject);
}