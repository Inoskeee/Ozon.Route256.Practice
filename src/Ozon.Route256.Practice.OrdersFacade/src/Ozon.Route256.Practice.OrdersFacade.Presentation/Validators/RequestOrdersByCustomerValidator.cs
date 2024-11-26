using FluentValidation;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Validators;

internal sealed class RequestOrdersByCustomerValidator : AbstractValidator<OrdersByCustomerRequestDto>
{
    public RequestOrdersByCustomerValidator()
    {
        RuleFor(x=>x.CustomerId)
            .GreaterThan(0)
            .WithMessage("'CustomerId' must be greater than zero");
        
        RuleFor(x=>x.Limit)
            .GreaterThan(0)
            .WithMessage("'Limit' must be greater than zero");
    }
}