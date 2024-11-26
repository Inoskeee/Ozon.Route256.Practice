using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

public sealed class CustomerInternalServerException : OrderFacadeExceptionBase
{
    public CustomerInternalServerException(string errorMessage)
    {
        ErrorCode = ErrorCodes.CUSTOMER_INTERNAL_ERROR;
        ErrorMessage = $"CustomerService request: {errorMessage}";
    }
}