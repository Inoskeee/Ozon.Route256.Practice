using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Customers;

public sealed class ExtendCustomerDto : CustomerDto
{
    [JsonPropertyName("region")]
    public required RegionDto Region { get; init; }
}