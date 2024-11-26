using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Exceptions;

public abstract class OrderFacadeExceptionBase : Exception
{
    public ErrorCodes ErrorCode { get; init;  }
    
    public string ErrorMessage { get; set; } = string.Empty;
}