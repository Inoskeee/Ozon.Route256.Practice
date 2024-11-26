using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;

public sealed class OrdersByRegionRequestDto : BaseRequestDto
{
    [JsonPropertyName("regionId")]
    [JsonPropertyOrder(1)]
    public int RegionId { get; set; }
}