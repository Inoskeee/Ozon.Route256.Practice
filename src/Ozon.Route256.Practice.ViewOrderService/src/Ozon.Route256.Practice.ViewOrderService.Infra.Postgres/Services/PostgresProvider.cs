using System.Text.Json;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.ViewOrderService.Bll.Contracts.ExternalServices;
using Ozon.Route256.Practice.ViewOrderService.Bll.Models;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Mappers;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Contracts.Repositories;

namespace Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Services;

internal class PostgresProvider : IPostgresProvider
{
    private readonly ILogger<PostgresProvider> _logger;
    
    private readonly IOrdersRepository _ordersRepository;

    private readonly IOrderMapper _orderMapper;
    public PostgresProvider(
        IOrdersRepository ordersRepository, 
        IOrderMapper orderMapper, 
        ILogger<PostgresProvider> logger)
    {
        _ordersRepository = ordersRepository;
        _orderMapper = orderMapper;
        _logger = logger;
    }

    public async Task UpdateOrInsertOrder(OrderModel order, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateOrInsertOrder | Поступил запрос на добавление заказа с Id: {orderId}", 
            order.OrderId);
        
        var orderEntity = _orderMapper.MapModelToEntity(order);

        var getOrderResult = await _ordersRepository.GetOrderById(orderEntity.OrderId, cancellationToken);

        if (getOrderResult is not null)
        {
            _logger.LogInformation("UpdateOrInsertOrder | Заказ с Id: {orderId} найден в БД. Выполняем запрос на обновление...",
                orderEntity.OrderId);
            
            await _ordersRepository.UpdateOrder(orderEntity, cancellationToken);
        }
        else
        {
            await _ordersRepository.AddOrder(orderEntity, cancellationToken);
            
            _logger.LogInformation("UpdateOrInsertOrder | В БД добавлен новый заказ с Id: {orderId}",
                orderEntity.OrderId);
        }
    }
}