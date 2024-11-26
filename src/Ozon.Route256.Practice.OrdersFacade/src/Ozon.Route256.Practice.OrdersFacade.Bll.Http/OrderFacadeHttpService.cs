using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Metrics;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.AggregateEntities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Mappers.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Http;

public class OrderFacadeHttpService(
    ILogger<OrderFacadeHttpService> logger,
    ICustomerHttpClientProvider customerHttpClient,
    IOrderHttpClientProvider orderHttpClient,
    IHttpCustomerMapper customerMapper,
    IHttpOrderMapper orderMapper,
    IAggregateByCustomerBuilder aggregateByCustomerBuilder,
    IAggregateByRegionBuilder aggregateByRegionBuilder,
    IOrdersFacadeMeter ordersFacadeMeter)
    : IOrderFacadeHttpService
{
    public async Task<AggregateByCustomerEntity> GetOrderByCustomer(long customerId, int limit, int offset)
    {
        ordersFacadeMeter.IncrementGetOrdersByCustomerCounter();
        var stopwatch = Stopwatch.StartNew();
        
        logger.LogInformation("Поступил запрос на получение заказов пользователя с id : {customerId}", 
            customerId);

        var orders = await orderHttpClient.GetOrdersByCustomerAsync(customerId, limit, offset);
        List<OrderEntityWithCustomerId> orderEntities = orders
            .Select(order=>  orderMapper.MapToEntity(order))
            .ToList();
        
        logger.LogInformation("Найдено заказов для пользователя {customerId} : {ordersCount}", 
            customerId, 
            orderEntities.Count);
        
        var customers = await customerHttpClient.GetCustomersAsync(new long[]{ customerId }, limit, offset);
        var customerEntities = customers.Customers
            .Select(customer => customerMapper.MapToEntity(customer))
            .ToList();

        var aggregateResult = aggregateByCustomerBuilder.Build(orderEntities, customerEntities);

        var delay = stopwatch.ElapsedMilliseconds;
        ordersFacadeMeter.MetricGetOrdersByCustomerDelay(delay);
        
        return aggregateResult;
    }

    public async Task<AggregateByRegionEntity> GetOrderByRegion(long regionId, int limit, int offset)
    {
        ordersFacadeMeter.IncrementGetOrdersByRegionCounter();
        var stopwatch = Stopwatch.StartNew();
        
        logger.LogInformation("Поступил запрос на получение заказов пользователя с id : {regionId}", 
            regionId);

        var orders = await orderHttpClient.GetOrdersByRegionAsync(regionId, limit, offset);
        List<OrderEntityWithCustomerId> orderEntities = orders
            .Select(order=>  orderMapper.MapToEntity(order))
            .ToList();

        logger.LogInformation("Получено заказов для региона {regionId} : {ordersCount}", 
            regionId,
            orderEntities.Count);

        long[] customerIds = orderEntities.Select(order => order.CustomerId).ToArray();

        var customers = await customerHttpClient.GetCustomersAsync(customerIds, limit, offset);
        var customerEntities = customers.Customers
            .Select(customer => customerMapper.MapToEntity(customer))
            .ToList();

        var aggregateResult = aggregateByRegionBuilder.Build(orderEntities, customerEntities);
        
        var delay = stopwatch.ElapsedMilliseconds;
        ordersFacadeMeter.MetricGetOrdersByRegionDelay(delay);
        
        return aggregateResult;
    }
}