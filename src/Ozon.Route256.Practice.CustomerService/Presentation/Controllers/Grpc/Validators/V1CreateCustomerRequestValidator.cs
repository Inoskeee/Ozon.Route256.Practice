using FluentValidation;

namespace Ozon.Route256.CustomerService.Presentation.Controllers.Grpc.Validators;

public sealed class V1CreateCustomerRequestValidator : AbstractValidator<V1CreateCustomerRequest>
{
    public V1CreateCustomerRequestValidator()
    {
        RuleFor(r => r.RegionId)
            .GreaterThan(0)
            .WithMessage("Region id should be greater than zero.");
        RuleFor(r => r.FullName)
            .Must((name) => name is { Length: > 0 and <= 256 })
            .WithMessage("Full name should be not empty and length should be less or equal 256 symbols");
    }
}