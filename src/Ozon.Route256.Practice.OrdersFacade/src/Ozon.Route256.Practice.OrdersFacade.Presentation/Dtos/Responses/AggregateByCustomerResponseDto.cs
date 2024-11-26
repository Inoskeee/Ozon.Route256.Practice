using System.Text.Json.Serialization;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Customers;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Orders;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;

public sealed class AggregateByCustomerResponseDto
{
    [JsonPropertyName("customer")]
    public ExtendCustomerDto? Customer { get; init; }
    
    [JsonPropertyName("orders")]
    public List<OrderEntityWithCustomerIdDto> Orders { get; init; } = new List<OrderEntityWithCustomerIdDto>();
}