using Ozon.Route256.OrderService.Proto;

namespace Ozon.Route256.OrderService.Bll.Exceptions;

public class InvalidOrderStatusException : OrderServiceExceptionBase
{
    public InvalidOrderStatusException()
    {
        ErrorCode = ErrorCode.InvalidOrderStatus;

        Message = "Invalid order status";
    }
}