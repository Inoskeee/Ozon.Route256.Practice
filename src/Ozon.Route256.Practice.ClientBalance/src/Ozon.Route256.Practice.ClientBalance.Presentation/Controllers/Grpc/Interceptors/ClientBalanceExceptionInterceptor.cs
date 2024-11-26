using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Controllers.Grpc.Interceptors;

public class ClientBalanceExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var result = await continuation(request, context);
            return result;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.ToString()));
        }
    }
}