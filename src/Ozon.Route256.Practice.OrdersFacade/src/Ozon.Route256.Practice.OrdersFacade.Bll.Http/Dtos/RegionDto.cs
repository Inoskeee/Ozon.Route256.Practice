using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos;

public sealed class RegionDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")] 
    public string Name { get; set; } = null!;
}