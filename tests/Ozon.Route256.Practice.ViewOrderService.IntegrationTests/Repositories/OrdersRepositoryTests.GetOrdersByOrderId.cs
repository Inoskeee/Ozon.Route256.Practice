﻿using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    
    [Fact]
    public async Task GetOrdersByOrderId_WhenOrdersExist_ReturnsOrders()
    {
        // Arrange
        List<OrderEntity> orderEntities = new List<OrderEntity>();

        for (int i = 0; i < 3; i++)
        {
            var order = OrderEntityCreator.Generate();
            orderEntities.Add(order);
        }
        
        foreach (var order in orderEntities)
        {
            await _repository.AddOrder(order, default);
        }

        var orderIds = orderEntities.Select(o => o.OrderId).ToArray();

        // Act
        var result = await _repository.GetOrdersByOrderId(orderIds, default);

        // Assert
        result.Should().HaveCount(3);
        result.Select(o => o.OrderId).Should().BeEquivalentTo(orderIds);
    }
    
    [Fact]
    public async Task GetOrdersByOrderId_WhenNoOrdersExist_ReturnsEmptyList()
    {
        // Arrange
        var orderIds = new long[] { 123, 456, 789 };

        // Act
        var result = await _repository.GetOrdersByOrderId(orderIds, default);

        // Assert
        result.Should().BeEmpty();
    }
}