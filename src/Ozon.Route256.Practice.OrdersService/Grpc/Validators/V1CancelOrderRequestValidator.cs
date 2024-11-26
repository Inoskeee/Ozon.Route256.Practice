using FluentValidation;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.Grpc.Validators;

public class V1CancelOrderRequestValidator : AbstractValidator<V1CancelOrderRequest>
{
    public V1CancelOrderRequestValidator()
    {
        RuleFor(query => query.OrderId)
            .GreaterThan(0);
    }
}