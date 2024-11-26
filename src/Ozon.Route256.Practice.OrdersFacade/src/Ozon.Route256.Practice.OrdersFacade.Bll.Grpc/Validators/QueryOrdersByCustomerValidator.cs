using FluentValidation;
using Ozon.Route256.Practice.OrderFacade;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Validators;

internal sealed class QueryOrdersByCustomerValidator : AbstractValidator<GetOrderByCustomerRequest>
{
    public QueryOrdersByCustomerValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("'CustomerId' must be greater than zero");
        
        RuleFor(x=>x.Limit)
            .GreaterThan(0)
            .WithMessage("'Limit' must be greater than zero");
    }
}