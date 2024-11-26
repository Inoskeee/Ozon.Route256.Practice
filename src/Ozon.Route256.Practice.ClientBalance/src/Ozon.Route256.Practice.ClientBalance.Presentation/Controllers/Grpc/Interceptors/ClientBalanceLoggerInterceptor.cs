using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Controllers.Grpc.Interceptors;

public class ClientBalanceLoggerInterceptor : Interceptor
{
    private readonly ILogger<ClientBalanceLoggerInterceptor> _logger;

    public ClientBalanceLoggerInterceptor(ILogger<ClientBalanceLoggerInterceptor> logger)
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
            
            _logger.LogInformation("Received gRPC response {method} after {elapsedMilliseconds}ms.", 
                context.Method, stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (RpcException)
        {
            stopwatch.Stop();
            _logger.LogError("Received gRPC response {method} after {elapsedMilliseconds}ms.", 
                context.Method, stopwatch.ElapsedMilliseconds);
            
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError("Received gRPC response {method} after {elapsedMilliseconds}ms.",
                context.Method, stopwatch.ElapsedMilliseconds);
            
            throw new RpcException(new Status(StatusCode.Internal, ex.ToString()));
        }
    }
}