using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Interceptors;

internal sealed class OrderFacadeExceptionInterceptor : Interceptor
{
    private readonly ILogger<OrderFacadeExceptionInterceptor> _logger;

    public OrderFacadeExceptionInterceptor(ILogger<OrderFacadeExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var result = await continuation(request, context);
            return result;
        }
        catch (CustomerNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}