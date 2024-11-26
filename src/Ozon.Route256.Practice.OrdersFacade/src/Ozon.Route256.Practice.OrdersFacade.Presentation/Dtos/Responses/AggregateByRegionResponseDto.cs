using System.Text.Json.Serialization;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Orders;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;

public sealed class AggregateByRegionResponseDto
{
    [JsonPropertyName("region")]
    public required RegionDto Region { get; set; }
    
    [JsonPropertyName("orders")]
    public List<OrderEntityWithCustomerDto> Orders { get; init; } = new List<OrderEntityWithCustomerDto>();
}