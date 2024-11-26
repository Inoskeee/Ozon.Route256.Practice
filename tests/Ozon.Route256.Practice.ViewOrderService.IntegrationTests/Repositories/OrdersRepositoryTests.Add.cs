using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    [Fact]
    public async Task AddOrder_WhenCalled_AddsOrderSuccessfully()
    {
        // Arrange
        var expected = OrderEntityCreator
            .Generate()
            .WithCreatedAt(DateTime.UtcNow);

        // Act
        await _repository.AddOrder(expected, default);

        // Assert
        var result = await _repository.GetOrderById(expected.OrderId, default);
        result.Should().NotBeNull();
        result?.OrderId.Should().Be(expected.OrderId);
    }
}