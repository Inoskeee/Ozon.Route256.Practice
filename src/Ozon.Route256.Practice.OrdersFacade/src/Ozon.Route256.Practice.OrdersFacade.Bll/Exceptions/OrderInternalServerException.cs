using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

public sealed class OrderInternalServerException : OrderFacadeExceptionBase
{
    public OrderInternalServerException(string errorMessage)
    {
        ErrorCode = ErrorCodes.ORDER_INTERNAL_ERROR;
        ErrorMessage = $"OrderService request: {errorMessage}";
    }
}