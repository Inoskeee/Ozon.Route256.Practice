using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersReport.Presentation.Dtos;

public class ReportByCustomerResponseDto
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;
}