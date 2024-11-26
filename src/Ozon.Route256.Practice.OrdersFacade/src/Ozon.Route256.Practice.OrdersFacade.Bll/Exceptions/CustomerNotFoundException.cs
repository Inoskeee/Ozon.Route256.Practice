using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

public sealed class CustomerNotFoundException : OrderFacadeExceptionBase
{
    public CustomerNotFoundException(long customerId)
    {
        ErrorCode = ErrorCodes.CUSTOMER_NOT_FOUND;
        ErrorMessage = $"Customer {customerId} not found";
    }
    
}