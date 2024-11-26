using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http.Dtos.HttpResponses;

public sealed class CustomersHttpResponseDto
{
    [JsonPropertyName("customers")]
    public List<CustomerDto> Customers { get; init; } = new List<CustomerDto>();
    
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }
}