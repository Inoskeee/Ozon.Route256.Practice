using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos;

public sealed class CustomerDto
{
    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }
    
    [JsonPropertyName("region")]
    public required RegionDto Region { get; set; }

    [JsonPropertyName("fullName")] 
    public string FullName { get; set; } = null!;
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}