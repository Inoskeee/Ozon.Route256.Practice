using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersReport.Presentation.Dtos;

public class ReportByCustomerRequestDto
{
    [JsonPropertyName("customer_id")]
    public long CustomerId { get; set; }
}