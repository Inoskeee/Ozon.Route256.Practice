using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpRequests;

public sealed class CustomersHttpRequestDto
{
    [JsonPropertyName("customerIds")] 
    public List<long> CustomerIds { get; set; } = new List<long>();

    [JsonPropertyName("regionIds")] 
    public List<int> RegionIds { get; set; } = new List<int>();

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }
}