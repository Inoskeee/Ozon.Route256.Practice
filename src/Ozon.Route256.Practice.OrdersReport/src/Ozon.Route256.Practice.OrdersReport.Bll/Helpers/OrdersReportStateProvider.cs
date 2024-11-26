using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrdersReport.Bll.Configuration;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Helpers;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Helpers;

public class OrdersReportStateProvider : IOrdersReportStateProvider, IDisposable
{
    public SemaphoreSlim RateLimitSemaphore { get; }
    public ConcurrentDictionary<long, CancellationTokenSource?> ActiveRequests { get; }
    
    public OrdersReportStateProvider(IOptions<RateLimitConfig> rateLimitConfig)
    {
        RateLimitSemaphore = new SemaphoreSlim(rateLimitConfig.Value.ConcurrentRequestsCount);
        ActiveRequests = new ConcurrentDictionary<long, CancellationTokenSource?>();
    }

    public void Dispose()
    {
        RateLimitSemaphore.Dispose();
    }
}