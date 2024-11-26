using Ozon.Route256.OrderService.Proto;

namespace Ozon.Route256.OrderService.Bll.Exceptions;

public class UnsupportedRegionException : OrderServiceExceptionBase
{
    public UnsupportedRegionException(long regionId)
    {
        ErrorCode = ErrorCode.UnsupportedRegion;

        Message = $"Unsupported region: {regionId}";
    }
}