using MediatR;
using Ozon.Route256.TestService.Data;
using Ozon.Route256.TestService.Domain.Metrics;

namespace Ozon.Route256.TestService.Domain.Actions.ListMismatches;

public class ListMismatchesHandler : IRequestHandler<ListMismatchesQuery, MismatchStatistics>
{
    private readonly IMismatchRepository _mismatchRepository;
    private readonly IMismatchStatisticsReader _statisticsReader;

    public ListMismatchesHandler(IMismatchRepository mismatchRepository, IMismatchStatisticsReader statisticsReader)
    {
        _mismatchRepository = mismatchRepository;
        _statisticsReader = statisticsReader;
    }

    public Task<MismatchStatistics> Handle(ListMismatchesQuery request, CancellationToken cancellationToken)
    {
        var mismatches = _mismatchRepository.ListAll();
        var statistics = _statisticsReader.GetStatistics();
        var (validCount, invalidCount) = CountValidAndInvalid(statistics);

        var result = new MismatchStatistics
        {
            Mismatches = mismatches,
            MismatchDistribution = statistics,
            ValidCount = validCount,
            InvalidCount = invalidCount,
        };

        return Task.FromResult(result);
    }

    private static (int validCount, int invalidCount) CountValidAndInvalid(IReadOnlyDictionary<MismatchType, int> statistics)
    {
        var invalidCount = 0;
        var validCount = 0;

        foreach (var (mismatchType, count) in statistics)
        {
            if (mismatchType == MismatchType.None)
            {
                validCount = count;
                continue;
            }

            invalidCount += count;
        }

        return (validCount, invalidCount);
    }
}
