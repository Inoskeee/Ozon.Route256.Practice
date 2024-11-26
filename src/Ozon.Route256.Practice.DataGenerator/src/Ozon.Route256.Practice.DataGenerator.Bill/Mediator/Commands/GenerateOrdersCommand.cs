using MediatR;

namespace Ozon.Route256.DataGenerator.Bll.Mediator.Commands;

public record GenerateOrdersCommand : IRequest<int>
{
    public required int OrdersCount { get; init; }

    public required int CustomersCount { get; init; }

    public required IReadOnlyList<int> BrokenOrdersIndexes { get; init; }
}
