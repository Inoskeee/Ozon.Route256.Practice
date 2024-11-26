namespace Ozon.Route256.OrderService.Dal.Models;

public class OrderQueryModel
{
    public long[] OrderIds { get; init; } = Array.Empty<long>();

    public long[] CustomerIds { get; init; } = Array.Empty<long>();

    public long[] RegionIds { get; init; } = Array.Empty<long>();

    public int Limit { get; init; }

    public int Offset { get; init; }

    public bool IsEmpty()
    {
        return OrderIds is not { Length: > 0 } &&
               CustomerIds is not { Length: > 0 } &&
               RegionIds is not { Length: > 0 };
    }
}