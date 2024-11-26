using System.Text.Json.Serialization;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Customers;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Orders;

public sealed class OrderEntityWithCustomerDto : BaseOrderDto
{
    [JsonPropertyName("customer")]
    public required CustomerDto Customer { get; set; }
}