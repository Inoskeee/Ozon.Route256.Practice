using Ozon.Route256.OrderService.Proto;

namespace Ozon.Route256.OrderService.Bll.Exceptions;

public class InvalidItemsCountException : OrderServiceExceptionBase
{
    public InvalidItemsCountException()
    {
        ErrorCode = ErrorCode.InvalidItemsCount;

        Message = "Quantity of the items must be greater than 0";
    }
}