using FluentValidation;

namespace Ozon.Route256.CustomerService.Presentation.Controllers.Grpc.Validators;

public sealed class V1QueryCustomersRequestValidator : AbstractValidator<V1QueryCustomersRequest>
{
    public V1QueryCustomersRequestValidator()
    {
        RuleFor(r => r.Limit)
            .GreaterThan(0)
            .WithMessage("Limit should be greater than zero.");
        RuleFor(r => r.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Offset should be greater than or equal to zero");
        RuleForEach(r => r.CustomerIds)
            .GreaterThan(0)
            .WithMessage("Customer id should be greater than zero");
        RuleForEach(r => r.RegionIds)
            .GreaterThan(0)
            .WithMessage("Region id should be greater than zero");
    }
}