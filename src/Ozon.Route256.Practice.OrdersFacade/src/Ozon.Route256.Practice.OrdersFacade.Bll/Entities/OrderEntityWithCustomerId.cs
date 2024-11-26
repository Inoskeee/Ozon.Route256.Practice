using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

public sealed class OrderEntityWithCustomerId : BaseOrderEntity
{
    public long CustomerId { get; init; }
}