using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;

public sealed class OrdersByCustomerRequestDto : BaseRequestDto
{
    [JsonPropertyName("customerId")]
    [JsonPropertyOrder(1)]
    public int CustomerId { get; set; } 
}