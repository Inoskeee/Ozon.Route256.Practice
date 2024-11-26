using Ozon.Route256.TestService.Data.Orders;

namespace Ozon.Route256.TestService.Domain.Actions.MatchOrderError;

public class StoredData
{
    public required IReadOnlyCollection<Order> Orders { get; init; }
}
