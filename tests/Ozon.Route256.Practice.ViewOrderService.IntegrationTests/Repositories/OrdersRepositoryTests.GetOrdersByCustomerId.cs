using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    
    [Fact]
    public async Task GetOrdersByCustomerId_WhenOrdersExist_ReturnsOrders()
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

        var customerIds = orderEntities.Select(o => o.CustomerId).Distinct().ToArray();

        // Act
        var result = await _repository.GetOrdersByCustomerId(customerIds, default);

        // Assert
        result.Select(o => o.OrderId).Should().BeEquivalentTo(orderEntities.Select(o => o.OrderId));
    }

    [Fact]
    public async Task GetOrdersByCustomerId_WhenNoOrdersExist_ReturnsEmptyList()
    {
        // Arrange
        var customerIds = new long[] { 999 };

        // Act
        var result = await _repository.GetOrdersByCustomerId(customerIds, default);

        // Assert
        result.Should().BeEmpty();
    }
}