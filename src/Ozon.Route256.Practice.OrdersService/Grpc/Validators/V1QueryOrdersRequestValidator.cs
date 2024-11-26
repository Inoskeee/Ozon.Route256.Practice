using FluentValidation;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.Grpc.Validators;

public class V1QueryOrdersRequestValidator : AbstractValidator<V1QueryOrdersRequest>
{
    public V1QueryOrdersRequestValidator()
    {
        RuleFor(query => query.Limit)
            .GreaterThan(0)
            .WithMessage("Page size should be greater than 0");

        RuleFor(query => query.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page number should not be less than 0");

        RuleForEach(query => query.OrderIds)
            .GreaterThan(0)
            .WithMessage("OrderId should be greater than 0");
        
        RuleForEach(query => query.CustomerIds)
            .GreaterThan(0)
            .WithMessage("CustomerId should be greater than 0");
        
        RuleForEach(query => query.RegionIds)
            .GreaterThan(0)
            .WithMessage("RegionId should be greater than 0");
    }
}