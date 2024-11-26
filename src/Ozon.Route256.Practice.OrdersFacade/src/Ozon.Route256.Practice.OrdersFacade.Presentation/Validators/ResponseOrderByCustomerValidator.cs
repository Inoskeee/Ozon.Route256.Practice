using Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Requests;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Dtos.Responses;

namespace Ozon.Route256.Practice.OrdersFacade.Presentation.Validators;

internal sealed class ResponseOrderByCustomerValidator : IResponseValidatorProvider<OrdersByCustomerRequestDto, AggregateByCustomerResponseDto>
{
    public void Validate(OrdersByCustomerRequestDto request, AggregateByCustomerResponseDto response)
    {
        if (response.Customer == null)
        {
            throw new CustomerNotFoundException(request.CustomerId);
        }
    }
}