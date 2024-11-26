using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Infrastructure;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggerMiddleware> _logger;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Received HTTP request {context.Request.Path}");

        Stopwatch stopwatch = Stopwatch.StartNew();
        
        await _next(context);
        
        stopwatch.Stop();
            
        _logger.LogInformation($"Received HTTP response {context.Request.Path} after {stopwatch.ElapsedMilliseconds}ms.");
    }

}