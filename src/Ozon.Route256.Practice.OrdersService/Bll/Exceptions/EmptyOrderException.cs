using Ozon.Route256.OrderService.Proto;

namespace Ozon.Route256.OrderService.Bll.Exceptions;

public class EmptyOrderException : OrderServiceExceptionBase
{
    public EmptyOrderException()
    {
        ErrorCode = ErrorCode.EmptyOrder;
        
        Message = "Order without items";
    }
}