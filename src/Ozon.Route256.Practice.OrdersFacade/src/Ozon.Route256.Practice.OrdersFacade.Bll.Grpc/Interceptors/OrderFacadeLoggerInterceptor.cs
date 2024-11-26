using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Interceptors;

internal sealed class OrderFacadeLoggerInterceptor : Interceptor
{
    private readonly ILogger<OrderFacadeLoggerInterceptor> _logger;
    
    public OrderFacadeLoggerInterceptor(ILogger<OrderFacadeLoggerInterceptor> logger)
    {
        _logger = logger;
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Received gRPC request {method}", context.Method);

        Stopwatch stopwatch = Stopwatch.StartNew();
        try
        {
            var result = await continuation(request, context);

            stopwatch.Stop();
            
            _logger.LogInformation("Received gRPC response {method} after {milliseconds}ms.", 
                context.Method, 
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (RpcException ex)
        {
            stopwatch.Stop();
            _logger.LogError("Received gRPC response {method} after {milliseconds}ms with exception [{exception}]", 
                context.Method, 
                stopwatch.ElapsedMilliseconds, 
                ex.ToString());
            
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            _logger.LogError("Received gRPC response {method} after {milliseconds}ms with exception [{exception}]", 
                context.Method, 
                stopwatch.ElapsedMilliseconds, 
                ex.ToString());
            
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}