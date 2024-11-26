using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Orders;

public abstract class BaseOrderDto
{
    [JsonPropertyName("orderId")]
    public long OrderId { get; set; }
    
    [JsonPropertyName("region")]
    public required RegionDto Region { get; init; }

    [JsonPropertyName("status")] 
    public string Status { get; init; } = string.Empty;
    
    [JsonPropertyName("comment")]
    public string Comment { get; init; } = string.Empty;
    
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; }
}