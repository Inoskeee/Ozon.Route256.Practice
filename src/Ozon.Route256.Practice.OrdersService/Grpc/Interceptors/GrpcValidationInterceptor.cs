using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.Route256.OrderService.Grpc.Interceptors;

public class GrpcValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public GrpcValidationInterceptor(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var validators = _serviceProvider.GetServices<IValidator<TRequest>>();
            foreach (var validator in validators)
            {
                var validationResult = await validator.ValidateAsync(request, context.CancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }

            var response = await continuation(request, context);
            return response;
        }
        catch (ValidationException exception)
        {
            throw new RpcException(new Status(StatusCode.FailedPrecondition, exception.Message));
        }
    }
}