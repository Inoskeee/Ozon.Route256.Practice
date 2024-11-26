using MediatR;

namespace Ozon.Route256.TestService.Domain.Actions.ListMismatches;

public class ListMismatchesQuery : IRequest<MismatchStatistics>
{
}
