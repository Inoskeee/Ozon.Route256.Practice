using System.Collections.Concurrent;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Helpers;

public interface IOrdersReportStateProvider
{
    SemaphoreSlim RateLimitSemaphore { get; }
    ConcurrentDictionary<long, CancellationTokenSource?> ActiveRequests { get; }
}