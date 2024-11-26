namespace Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Metrics;

public interface IOrdersFacadeMeter
{
    void IncrementGetOrdersByCustomerCounter(int delta = 1);
    void MetricGetOrdersByCustomerDelay(long value);
    
    void IncrementGetOrdersByRegionCounter(int delta = 1);
    
    void MetricGetOrdersByRegionDelay(long value);
}