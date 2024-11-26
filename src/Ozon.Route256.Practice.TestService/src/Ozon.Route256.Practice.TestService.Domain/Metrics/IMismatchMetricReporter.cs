using Ozon.Route256.TestService.Data;

namespace Ozon.Route256.TestService.Domain.Metrics;

public interface IMismatchMetricReporter
{
    void Inc(MismatchType mismatchType);

    void Clear();
}
