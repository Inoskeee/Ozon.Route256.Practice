using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;

public sealed class OrdersHttpResponseDto
{
    [JsonPropertyName("orderId")]
    public int OrderId { get; init; }

    [JsonPropertyName("region")]
    public required RegionDto Region { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = null!;

    [JsonPropertyName("customerId")]
    public int CustomerId { get; init; }

    [JsonPropertyName("comment")]
    public string Comment { get; init; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}