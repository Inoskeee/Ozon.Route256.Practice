using Ozon.Route256.TestService.Data;

namespace Ozon.Route256.TestService.Domain.Metrics;

public class MismatchMetricManager : IMismatchMetricReporter, IMismatchStatisticsReader
{
    private readonly object _lockObject = new();

    private Dictionary<MismatchType, int> _statistics = CreateStatistics();

    public void Inc(MismatchType mismatchType)
    {
        lock (_lockObject)
        {
            _statistics[mismatchType] += 1;
        }
    }

    public void Clear()
    {
        lock (_lockObject)
        {
            _statistics = CreateStatistics();
        }
    }

    public IReadOnlyDictionary<MismatchType, int> GetStatistics()
    {
        lock (_lockObject)
        {
            var dictionary = new Dictionary<MismatchType, int>(_statistics);

            return dictionary;
        }
    }

    private static Dictionary<MismatchType, int> CreateStatistics() =>
        new(
            Enum
                .GetValues<MismatchType>()
                .Select(mismatchType => new KeyValuePair<MismatchType, int>(mismatchType, 0)));
}
