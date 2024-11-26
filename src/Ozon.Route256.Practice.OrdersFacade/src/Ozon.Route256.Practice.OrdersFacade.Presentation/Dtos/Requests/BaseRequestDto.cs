using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;

public abstract class BaseRequestDto
{
    [JsonPropertyName("limit")]
    [JsonPropertyOrder(2)]
    public int Limit { get; set; }
    
    [JsonPropertyName("offset")]
    [JsonPropertyOrder(3)]
    public int Offset { get; set; }
}