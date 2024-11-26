using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;

internal sealed class ErrorResponseDto
{
    [JsonPropertyName("code")]
    public int ErrorCode { get; set; }
    
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;
    
}