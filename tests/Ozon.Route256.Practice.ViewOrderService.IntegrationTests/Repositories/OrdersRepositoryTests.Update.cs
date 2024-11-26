using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    [Fact]
    public async Task UpdateOrder_WhenOrderExists_UpdatesOrder()
    {
        // Arrange
        var originalOrder = OrderEntityCreator.Generate().WithCreatedAt(DateTime.UtcNow);
        await _repository.AddOrder(originalOrder, default);

        // Act
        var updatedOrder = originalOrder with { Comment = "Updated Comment" };
        await _repository.UpdateOrder(updatedOrder, default);

        // Assert
        var result = await _repository.GetOrderById(originalOrder.OrderId, default);
        result.Should().NotBeNull();
        result?.Comment.Should().Be("Updated Comment");
    }

    [Fact]
    public async Task UpdateOrder_WhenOrderDoesNotExist_DoesNotThrow()
    {
        // Arrange
        var nonExistingOrder = OrderEntityCreator.Generate().WithCreatedAt(DateTime.UtcNow);

        // Act
        Func<Task> act = async () => await _repository.UpdateOrder(nonExistingOrder, default);

        // Assert
        await act.Should().NotThrowAsync();
        var result = await _repository.GetOrderById(nonExistingOrder.OrderId, default);
        result.Should().BeNull();
    }
}