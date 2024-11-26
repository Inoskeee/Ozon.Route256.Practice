using System.ComponentModel.DataAnnotations;

using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.Route256.CustomerService.Presentation.Interceptors;

public sealed class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await continuation(request, context);
            return response;
        }
        catch (ValidationException exception)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, exception.Message));
        }
        catch (Exception exception)
        {
            throw new RpcException(new Status(StatusCode.Internal, exception.Message));
        }
    }
}
