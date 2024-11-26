using System.Diagnostics;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrderFacade;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Metrics;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Mappers.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Services.Contracts;
using Ozon.Route256.Practice.OrdersFacade.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersFacade.Bll.Grpc;

public class OrderFacadeGrpcService(
    ILogger<OrderFacadeGrpcService> logger,
    IGrpcCustomerMapper customerMapper,
    IGrpcOrderMapper orderMapper,
    ICustomerGrpcClientProvider customerGrpcClientProvider,
    IOrderGrpcClientProvider orderGrpcClientProvider,
    IAggregateByCustomerBuilder aggregateByCustomerBuilder,
    IGrpcAggregateResponseMapper aggregateResponseMapper,
    IAggregateByRegionBuilder aggregateByRegionBuilder,
    IResponseValidatorProvider<GetOrderByCustomerRequest, GetOrderByCustomerResponse> customerResponseValidator,
    IOrdersFacadeMeter ordersFacadeMeter)
    : OrderFacadeGrpc.OrderFacadeGrpcBase
{
    public override async Task<GetOrderByCustomerResponse> GetOrderByCustomer(GetOrderByCustomerRequest request, ServerCallContext context)
    {
        ordersFacadeMeter.IncrementGetOrdersByCustomerCounter();
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation("Поступил запрос на получение заказов пользователя с id : {customerId}", 
            request.CustomerId);
        
        var orders = await orderGrpcClientProvider
            .GetOrdersByCustomerAsync(request.CustomerId, request.Limit, request.Offset);
        List<OrderEntityWithCustomerId> orderEntities = orders
            .Select(order=>  orderMapper.MapToEntity(order))
            .ToList();

        logger.LogInformation("Найдено заказов для пользователя {customerId} : {ordersCount}", 
            request.CustomerId, 
            orderEntities.Count);

        
        var customers = await customerGrpcClientProvider
            .GetCustomersAsync(new[] { request.CustomerId }, request.Limit, request.Offset);
        var customerEntities = customers.SelectMany(customerList =>
                customerList.Customers.Select(customer => customerMapper.MapToEntity(customer)))
            .ToList();
        
        var aggregateResult = aggregateByCustomerBuilder.Build(orderEntities, customerEntities);
        var aggregateResponse = aggregateResponseMapper.MapByCustomerToResponse(aggregateResult);
        
        customerResponseValidator.Validate(request, aggregateResponse);
        
        var delay = stopwatch.ElapsedMilliseconds;
        ordersFacadeMeter.MetricGetOrdersByCustomerDelay(delay);
        
        return aggregateResponse;
    }

    public override async Task<GetOrderByRegionResponse> GetOrderByRegion(GetOrderByRegionRequest request, ServerCallContext context)
    {
        ordersFacadeMeter.IncrementGetOrdersByRegionCounter();
        var stopwatch = Stopwatch.StartNew();
        
        logger.LogInformation("Поступил запрос на получение заказов пользователя с id : {regionId}", 
            request.RegionId);
        
        var orders = await orderGrpcClientProvider
            .GetOrdersByRegionAsync(request.RegionId, request.Limit, request.Offset);
        List<OrderEntityWithCustomerId> orderEntities = orders
            .Select(order=>  orderMapper.MapToEntity(order))
            .ToList();

        logger.LogInformation("Получено заказов для региона {regionId} : {ordersCount}", 
            request.RegionId,
            orderEntities.Count);

        long[] customerIds = orderEntities.Select(order => order.CustomerId).ToArray();

        var customers = await customerGrpcClientProvider.GetCustomersAsync(customerIds, request.Limit, request.Offset);
        var customerEntities = customers.SelectMany(customerList =>
                customerList.Customers.Select(customer => customerMapper.MapToEntity(customer)))
            .ToList();

        var aggregateResult = aggregateByRegionBuilder.Build(orderEntities, customerEntities);
        var aggregateResponse = aggregateResponseMapper.MapByRegionToResponse(aggregateResult);
        
        var delay = stopwatch.ElapsedMilliseconds;
        ordersFacadeMeter.MetricGetOrdersByRegionDelay(delay);
        
        return aggregateResponse;
    }
}