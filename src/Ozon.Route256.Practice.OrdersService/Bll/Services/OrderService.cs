using System.Transactions;
using Ozon.Route256.OrderService.Bll.Exceptions;
using Ozon.Route256.OrderService.Bll.Extensions;
using Ozon.Route256.OrderService.Bll.Helpers.Interfaces;
using Ozon.Route256.OrderService.Bll.Models;
using Ozon.Route256.OrderService.Bll.Services.Interfaces;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Interfaces;
using Ozon.Route256.OrderService.Dal.Models;
using Ozon.Route256.OrderService.Dal.Repositories;
using Ozon.Route256.OrderService.Kafka;
using Ozon.Route256.OrderService.Proto.Messages;
using Ozon.Route256.OrderService.Proto.OrderGrpc;
using InputOrder = Ozon.Route256.OrderService.Bll.Models.InputOrder;

namespace Ozon.Route256.OrderService.Bll.Services;

public class OrderService : IOrderService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IItemsRepository _itemsRepository;
    private readonly IInputValidationHelper _validationHelper;
    private readonly ILogsService _logsService;
    private readonly IOrderInputErrorsPublisher _errorsPublisher;
    private readonly IOrderOutputEventPublisher _eventPublisher;

    public OrderService(
        IOrdersRepository ordersRepository,
        ILogsService logsService,
        IOrderInputErrorsPublisher errorsPublisher,
        IOrderOutputEventPublisher eventPublisher,
        IInputValidationHelper validationHelper,
        IItemsRepository itemsRepository)
    {
        _ordersRepository = ordersRepository;
        _logsService = logsService;
        _errorsPublisher = errorsPublisher;
        _eventPublisher = eventPublisher;
        _validationHelper = validationHelper;
        _itemsRepository = itemsRepository;
    }

    public async Task CreateOrder(
        InputOrder inputOrder,
        CancellationToken token)
    {
        var validationResult = await _validationHelper.ValidateInputOrder(inputOrder, token);
        if (!validationResult.Success)
        {
            await _errorsPublisher.PublishToKafka(validationResult.Message, token);
            return;
        }

        using var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var orderIds = await _ordersRepository.Insert(
            new OrderEntity
            {
                RegionId = inputOrder.RegionId,
                CustomerId = inputOrder.CustomerId,
                Status = (int)OrderStatus.New,
                Comment = inputOrder.Comment,
                CreatedAt = DateTimeOffset.UtcNow
            },
            token);

        var orderId = orderIds.First();
        var itemEntities = inputOrder.Items.Select(item => item.ToDal(orderId)).ToArray();
        await _itemsRepository.Insert(itemEntities, token);
        
        ts.Complete();

        await _eventPublisher.PublishToKafka(
            new OrderOutputEventMessage
            {
                OrderId = orderId,
                EventType = OutputEventType.Created
            },
            token);
    }

    public async Task<(Order[] Orders, int TotalCount)> QueryOrders(
        long[] orderIds,
        long[] customerIds,
        long[] regionIds,
        int limit,
        int offset,
        CancellationToken token)
    {
        var orderEntities = await _ordersRepository.Query(
            new OrderQueryModel
            {
                OrderIds = orderIds,
                CustomerIds = customerIds,
                RegionIds = regionIds,
                Limit = limit,
                Offset = offset
            },
            token);

        var totalCount = orderEntities is { Length: > 0 } ? orderEntities.First().TotalCount : 0;

        return (Orders: orderEntities.Select(entity => entity.ToBll()).ToArray(), TotalCount: totalCount);
    }

    public async Task<Result> CancelOrder(
        long orderId, 
        CancellationToken token)
    {
        var orders = (await _ordersRepository.Query(
            new OrderQueryModel { OrderIds = orderId.MakeArray() },
            token)).Select(entity => entity.ToBll()).ToArray();

        var order = orders.FirstOrDefault();
        if (order is null)
        {
            return ReturnFailed(
                error: nameof(OrderNotFoundException),
                errorMessage: $"Order {orderId} is not found");
        }

        if (order.Status is OrderStatus.Delivered)
        {
            return ReturnFailed(
                error: nameof(InvalidOrderStatusException),
                errorMessage: "Invalid order status");
        }
        
        var orderToUpdate = order with { Status = OrderStatus.Canceled };
        await _ordersRepository.Update(orderToUpdate.ToDal(), token);
        await _logsService.AddLog(orderToUpdate, token);
        
        await _eventPublisher.PublishToKafka(
            new OrderOutputEventMessage
            {
                OrderId = orderToUpdate.OrderId,
                EventType = OutputEventType.Updated
            },
            token);

        return new Result(Success: true, Error: null);;
    }

    public async Task<Result> DeliveryOrder(
        long orderId,
        CancellationToken token)
    {
        var orders = (await _ordersRepository.Query(
            new OrderQueryModel { OrderIds = orderId.MakeArray() },
            token)).Select(entity => entity.ToBll()).ToArray();

        var order = orders.FirstOrDefault();
        if (order is null)
        {
            return ReturnFailed(
                error: nameof(OrderNotFoundException),
                errorMessage: $"Order {orderId} is not found");
        }
        
        if (order.Status is OrderStatus.Canceled)
        {
            return ReturnFailed(
                error: nameof(InvalidOrderStatusException),
                errorMessage: $"Invalid order status");
        }
        
        var orderToUpdate = order with { Status = OrderStatus.Delivered };
        await _ordersRepository.Update(orderToUpdate.ToDal(), token);
        await _logsService.AddLog(orderToUpdate, token);
        
        await _eventPublisher.PublishToKafka(
            new OrderOutputEventMessage
            {
                OrderId = orderToUpdate.OrderId,
                EventType = OutputEventType.Updated
            },
            token);

        return new Result(Success: true, Error: null);
    }

    private Result ReturnFailed(
        string error, 
        string errorMessage)
    {
        return new Result(
            Success: false,
            Error: new ActionError(
                ErrorCode: error,
                ErrorMessage: errorMessage));
    }
}