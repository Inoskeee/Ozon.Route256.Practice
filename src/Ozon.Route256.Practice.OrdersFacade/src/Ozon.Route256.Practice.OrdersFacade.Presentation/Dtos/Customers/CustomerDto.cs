using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Customers;

public class CustomerDto
{
    [JsonPropertyName("customerId")]
    public long CustomerId { get; init; }
    
    [JsonPropertyName("fullName")]
    public string FullName { get; init; } = string.Empty;
    
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }
}