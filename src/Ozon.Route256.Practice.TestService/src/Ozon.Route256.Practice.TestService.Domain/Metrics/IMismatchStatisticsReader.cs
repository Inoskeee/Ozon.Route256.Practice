using Ozon.Route256.TestService.Data;

namespace Ozon.Route256.TestService.Domain.Metrics;

public interface IMismatchStatisticsReader
{
    IReadOnlyDictionary<MismatchType, int> GetStatistics();
}
