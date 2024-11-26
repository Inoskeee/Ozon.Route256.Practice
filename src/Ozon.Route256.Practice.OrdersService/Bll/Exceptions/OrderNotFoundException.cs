using Ozon.Route256.OrderService.Proto;

namespace Ozon.Route256.OrderService.Bll.Exceptions;

public class OrderNotFoundException : OrderServiceExceptionBase
{
    
    public OrderNotFoundException(long orderId)
    {
        ErrorCode = ErrorCode.OrderNotFound;

        Message = $"Order {orderId} is not found";
    }
}