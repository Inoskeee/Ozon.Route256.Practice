using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    
    [Fact]
    public async Task GetOrdersByLimit_WhenOrdersExist_ReturnsLimitedOrders()
    {
        // Arrange
        List<OrderEntity> orderEntities = new List<OrderEntity>();

        for (int i = 0; i < 5; i++)
        {
            var order = OrderEntityCreator.Generate();
            orderEntities.Add(order);
        }
        
        foreach (var order in orderEntities)
        {
            await _repository.AddOrder(order, default);
        }

        long limit = 3;
        long offset = 0;

        // Act
        var result = await _repository.GetOrdersByLimit(limit, offset, default);
        
        // Assert
        result.Should().HaveCount((int)limit);
    }
    
    [Fact]
    public async Task GetOrdersByLimit_WhenOrdersExist_ReturnsAllOrders()
    {
        // Arrange
        List<OrderEntity> orderEntities = new List<OrderEntity>();

        for (int i = 0; i < 5; i++)
        {
            var order = OrderEntityCreator.Generate();
            orderEntities.Add(order);
        }
        
        foreach (var order in orderEntities)
        {
            await _repository.AddOrder(order, default);
        }

        long limit = 100;
        long offset = 0;

        // Act
        var result = await _repository.GetOrdersByLimit(limit, offset, default);
        
        // Assert
        result.Should().HaveCountLessOrEqualTo((int)limit);
    }
    
    [Fact]
    public async Task GetOrdersByLimit_WhenNoOrdersExist_ReturnsEmptyList()
    {
        // Arrange
        long limit = 10;
        long offset = 100;

        // Act
        var result = await _repository.GetOrdersByLimit(limit, offset, default);

        // Assert
        result.Should().BeEmpty();
    }
}