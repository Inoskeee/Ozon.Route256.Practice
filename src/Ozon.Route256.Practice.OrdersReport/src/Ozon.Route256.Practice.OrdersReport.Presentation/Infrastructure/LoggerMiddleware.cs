using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.Practice.OrdersReport.Presentation.Infrastructure;

public class LoggerMiddleware
{
    private readonly ILogger<LoggerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Received HTTP request {path}", context.Request.Path);

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation("Received HTTP response {path} after {elapsedMs}ms.",
            context.Request.Path, stopwatch.ElapsedMilliseconds);
    }
}