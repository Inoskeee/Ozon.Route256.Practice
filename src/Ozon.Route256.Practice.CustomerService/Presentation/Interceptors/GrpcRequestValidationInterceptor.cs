using System.Text;

using FluentValidation;

using Grpc.Core;
using Grpc.Core.Interceptors;

using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Ozon.Route256.CustomerService.Presentation.Interceptors;

public sealed class GrpcRequestValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public GrpcRequestValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        await ValidateRequest(request, context.CancellationToken);

        return await base.UnaryServerHandler(request, context, continuation);
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        await ValidateRequest(request, context.CancellationToken);

        await base.ServerStreamingServerHandler(request, responseStream, context, continuation);
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        await ValidateRequest(requestStream, context.CancellationToken);

        return await base.ClientStreamingServerHandler(requestStream, context, continuation);
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        await ValidateRequest(requestStream, context.CancellationToken);

        await base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
    }

    private async Task ValidateRequest<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : class
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator == null)
        {
            return;
        }

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.IsValid)
        {
            return;
        }

        throw new RpcException(new Status(StatusCode.InvalidArgument, BuiltMessage(result.Errors)));
    }

    private static string BuiltMessage(ICollection<ValidationFailure> errors)
    {
        if (errors.Count <= 0)
        {
            return string.Empty;
        }

        var fullMessage = new StringBuilder();
        foreach (var error in errors)
        {
            fullMessage.AppendLine(error.ErrorMessage);
        }

        return fullMessage.ToString();
    }
}