namespace Ozon.Route256.Practice.OrdersFacade.Bll.Entities;

public sealed class OrderEntityWithCustomerEntity : BaseOrderEntity
{
    public required CustomerEntity Customer { get; set; }
}