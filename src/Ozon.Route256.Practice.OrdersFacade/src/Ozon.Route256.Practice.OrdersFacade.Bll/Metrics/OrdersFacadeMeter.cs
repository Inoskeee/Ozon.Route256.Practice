using System.Diagnostics.Metrics;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Metrics;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Metrics;

public class OrdersFacadeMeter : IOrdersFacadeMeter
{
    public const string MeterName = "orders-facade-metrics";

    private readonly Counter<int> _getOrdersByCustomerCounter;
    private readonly Histogram<long> _getOrdersByCustomerDelay;
    
    private readonly Counter<int> _getOrdersByRegionCounter;
    private readonly Histogram<long> _getOrdersByRegionDelay;

    public OrdersFacadeMeter()
    {
        var meter = new Meter(MeterName);

        _getOrdersByCustomerCounter = meter.CreateCounter<int>("get-orders-by-customer-counter");
        _getOrdersByCustomerDelay = meter.CreateHistogram<long>("get-orders-by-customer-delay");
        
        _getOrdersByRegionCounter = meter.CreateCounter<int>("get-orders-by-region-counter");
        _getOrdersByRegionDelay = meter.CreateHistogram<long>("get-orders-by-region-delay");
    }

    public void IncrementGetOrdersByCustomerCounter(int delta = 1) => _getOrdersByCustomerCounter.Add(delta);
    
    public void MetricGetOrdersByCustomerDelay(long value) => _getOrdersByCustomerDelay.Record(value);

    public void IncrementGetOrdersByRegionCounter(int delta = 1) => _getOrdersByRegionCounter.Add(delta);

    public void MetricGetOrdersByRegionDelay(long value) => _getOrdersByRegionDelay.Record(value);
}