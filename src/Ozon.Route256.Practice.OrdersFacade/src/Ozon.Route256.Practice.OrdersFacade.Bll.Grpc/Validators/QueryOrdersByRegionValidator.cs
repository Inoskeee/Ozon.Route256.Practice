using FluentValidation;
using Ozon.Route256.Practice.OrderFacade;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Validators;

internal sealed class QueryOrdersByRegionValidator : AbstractValidator<GetOrderByRegionRequest>
{
    public QueryOrdersByRegionValidator()
    {
        RuleFor(x=>x.RegionId)
            .GreaterThan(0)
            .WithMessage("'RegionId' must be greater than zero");
        
        RuleFor(x=>x.Limit)
            .GreaterThan(0)
            .WithMessage("'Limit' must be greater than zero");
    }
}