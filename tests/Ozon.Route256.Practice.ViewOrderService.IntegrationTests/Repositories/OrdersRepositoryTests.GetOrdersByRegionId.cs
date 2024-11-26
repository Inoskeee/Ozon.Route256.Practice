using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Entities;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    
    [Fact]
    public async Task GetOrdersByRegionId_WhenOrdersExist_ReturnsOrders()
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

        var regionIds = orderEntities.Select(o => o.RegionId).Distinct().ToArray();

        // Act
        var result = await _repository.GetOrdersByRegionId(regionIds, default);

        // Assert
        result.Select(o => o.OrderId).Should().BeEquivalentTo(orderEntities.Select(o => o.OrderId));
    }
    
    [Fact]
    public async Task GetOrdersByRegionId_WhenNoOrdersExist_ReturnsEmptyList()
    {
        // Arrange
        var regionIds = new long[] { 999 };

        // Act
        var result = await _repository.GetOrdersByRegionId(regionIds, default);

        // Assert
        result.Should().BeEmpty();
    }
}