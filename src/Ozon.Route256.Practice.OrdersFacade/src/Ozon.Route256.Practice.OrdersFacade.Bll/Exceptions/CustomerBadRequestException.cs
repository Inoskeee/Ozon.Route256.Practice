using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

public sealed class CustomerBadRequestException : OrderFacadeExceptionBase
{
    public CustomerBadRequestException(string errorMessage)
    {
        ErrorCode = ErrorCodes.CUSTOMER_BAD_REQUEST;
        ErrorMessage = $"CustomerService request: {errorMessage}";
    }
}