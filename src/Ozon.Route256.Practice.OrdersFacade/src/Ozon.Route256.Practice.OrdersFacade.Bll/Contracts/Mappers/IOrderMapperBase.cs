using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;

public interface IOrderMapperBase<TMapFrom>
{
    public OrderEntityWithCustomerId MapToEntity(TMapFrom mapObject);
}