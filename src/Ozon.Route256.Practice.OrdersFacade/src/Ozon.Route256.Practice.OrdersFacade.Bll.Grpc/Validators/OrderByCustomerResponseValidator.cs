using Ozon.Route256.Practice.OrderFacade;
using Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Validators;

internal sealed class OrderByCustomerResponseValidator : IResponseValidatorProvider<GetOrderByCustomerRequest, GetOrderByCustomerResponse>
{
    public void Validate(GetOrderByCustomerRequest request, GetOrderByCustomerResponse response)
    {
        if (response.Customer == null)
        {
            throw new CustomerNotFoundException(request.CustomerId);
        }
    }
}