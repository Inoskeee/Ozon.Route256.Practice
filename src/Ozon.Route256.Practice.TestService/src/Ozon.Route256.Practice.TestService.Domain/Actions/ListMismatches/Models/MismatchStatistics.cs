using Ozon.Route256.TestService.Data;

namespace Ozon.Route256.TestService.Domain.Actions.ListMismatches;

public class MismatchStatistics
{
    public required IReadOnlyDictionary<MismatchType, int> MismatchDistribution { get; init; }

    public required int ValidCount { get; init; }

    public required int InvalidCount { get; init; }

    public int TotalCount => ValidCount + InvalidCount;

    public int ValidPercent => TotalCount > 0
        ? (int)(ValidCount * 1d / TotalCount * 100)
        : 0;

    public required IReadOnlyCollection<Mismatch> Mismatches { get; init; }
}
