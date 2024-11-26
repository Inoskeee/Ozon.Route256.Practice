using FluentValidation;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.Grpc.Validators;

public class V1DeliveryOrderRequestValidator : AbstractValidator<V1DeliveryOrderRequest>
{
    public V1DeliveryOrderRequestValidator()
    {
        RuleFor(query => query.OrderId)
            .GreaterThan(0);
    }
}