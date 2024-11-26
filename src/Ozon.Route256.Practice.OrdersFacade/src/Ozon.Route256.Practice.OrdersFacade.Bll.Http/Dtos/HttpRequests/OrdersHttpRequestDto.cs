using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpRequests;

public sealed class OrdersHttpRequestDto
{
    [JsonPropertyName("orderIds")] 
    public List<long> OrderIds { get; set; } = new List<long>();
    
    [JsonPropertyName("customerIds")] 
    public List<long> CustomerIds { get; set; } = new List<long>();

    [JsonPropertyName("regionIds")] 
    public List<long> RegionIds { get; set; } = new List<long>();
    
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }
}