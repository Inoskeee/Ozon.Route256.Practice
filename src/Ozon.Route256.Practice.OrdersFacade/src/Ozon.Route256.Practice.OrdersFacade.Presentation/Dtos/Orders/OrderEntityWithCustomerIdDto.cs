using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Orders;

public sealed class OrderEntityWithCustomerIdDto : BaseOrderDto
{
    [JsonPropertyName("customerId")]
    public long CustomerId { get; init; }
}