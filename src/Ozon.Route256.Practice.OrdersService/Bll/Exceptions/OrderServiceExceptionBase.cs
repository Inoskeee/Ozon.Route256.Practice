using Ozon.Route256.OrderService.Proto;

namespace Ozon.Route256.OrderService.Bll.Exceptions;

public class OrderServiceExceptionBase : Exception
{
    public ErrorCode ErrorCode { get; init; }

    public string Message { get; set; }
}