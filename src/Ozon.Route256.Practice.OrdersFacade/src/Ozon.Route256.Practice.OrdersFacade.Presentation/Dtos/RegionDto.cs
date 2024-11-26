using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos;

public sealed class RegionDto
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}