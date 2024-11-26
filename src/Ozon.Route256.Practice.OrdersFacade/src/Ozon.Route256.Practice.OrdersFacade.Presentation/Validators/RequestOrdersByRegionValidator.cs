using FluentValidation;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Validators;

internal sealed class RequestOrdersByRegionValidator : AbstractValidator<OrdersByRegionRequestDto>
{
    public RequestOrdersByRegionValidator()
    {
        RuleFor(x=>x.RegionId)
            .GreaterThan(0)
            .WithMessage("'RegionId' must be greater than zero");
        
        RuleFor(x=>x.Limit)
            .GreaterThan(0)
            .WithMessage("'Limit' must be greater than zero");
    }
}