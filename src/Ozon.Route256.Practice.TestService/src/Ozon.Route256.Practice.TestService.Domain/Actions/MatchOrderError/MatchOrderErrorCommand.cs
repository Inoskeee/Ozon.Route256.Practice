using MediatR;

namespace Ozon.Route256.TestService.Domain.Actions.MatchOrderError;

public class MatchOrderErrorCommand : IRequest
{
    public required string Key { get; init; }

    public required long CustomerId { get; init; }

    public required long RegionId { get; init; }

    public required IReadOnlyCollection<OrderItem> OrderItems { get; init; }
}
