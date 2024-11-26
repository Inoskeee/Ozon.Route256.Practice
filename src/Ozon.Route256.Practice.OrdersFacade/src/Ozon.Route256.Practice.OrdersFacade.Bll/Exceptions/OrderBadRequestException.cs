using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

public sealed class OrderBadRequestException : OrderFacadeExceptionBase
{
    public OrderBadRequestException(string errorMessage)
    {
        ErrorCode = ErrorCodes.ORDER_BAD_REQUEST;
        ErrorMessage = $"OrderService request: {errorMessage}";
    }
}