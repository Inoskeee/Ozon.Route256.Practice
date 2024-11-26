using FluentAssertions;
using Ozon.Route256.DataGenerator.Bll.Models;
using Ozon.Route256.DataGenerator.Bll.Models.Enums;
using Ozon.Route256.DataGenerator.Bll.Services;
using Random = Ozon.Route256.DataGenerator.Bll.Creators.Random;

namespace Ozon.Route256.DataGenerator.UnitTests.Tests;

public class BrokenServiceTest
{
    [Theory]
    [InlineData(0, 0, 10, new int[]{})]
    [InlineData(0, 3, 2, new int[]{})]
    [InlineData(10, 1, 5, new[]{ 0, 1, 2, 3, 4 })]
    [InlineData(10, 7, 5, new[]{ 3 })]
    [InlineData(20, 35, 50, new[]{ 14, 49 })]
    public void GetBrokenOrderIndexes_ShouldReturn_CorrectCollection(
        long totalOrdersCount,
        int period,
        int ordersCountToCreate,
        IReadOnlyList<int> expectedBrokenOrderIndexes)
    {
        // arrange
        var service = new BrokenOrderService();

        // act
        var actualBrokenOrderIndexes = service.GetBrokenOrderIndexes(
            totalOrdersCount,
            period,
            ordersCountToCreate);

        // assert
        actualBrokenOrderIndexes.Should().BeEquivalentTo(expectedBrokenOrderIndexes);
    }

    [Fact]
    public void BreakOrder_ShouldReturn_InvalidOrder()
    {
        // arrange
        var item = new Item
        {
            Barcode = "barcode",
            Quantity = 29
        };

        var order = new Order
        {
            RegionId = (int)Random.EnumValue<Region>(),
            CustomerId = 100,
            Comment = "Test",
            Items = [item]
        };

        var service = new BrokenOrderService();

        // act
        var brokenOrder = service.BreakOrder(order);

        // assert
        var brokenReason = GetBrokenReason(order, brokenOrder);

        switch (brokenReason)
        {
            case OrderBrokenReason.InvalidRegion:
                AssertInvalidRegion(brokenOrder);
                break;
            case OrderBrokenReason.EmptyItems:
                AssertEmptyItems(brokenOrder);
                break;
            case OrderBrokenReason.IncorrectItemsQuantity:
                AssertInvalidQuantity(brokenOrder);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(OrderBrokenReason));
        }
    }

    private OrderBrokenReason GetBrokenReason(Order order, Order brokenOrder)
    {
        if (order.RegionId != brokenOrder.RegionId) return OrderBrokenReason.InvalidRegion;

        return !brokenOrder.Items.Any()
            ? OrderBrokenReason.EmptyItems
            : OrderBrokenReason.IncorrectItemsQuantity;
    }

    private static void AssertInvalidRegion(Order brokenOrder)
    {
        var regions = Enum
            .GetValues<Region>()
            .Cast<int>()
            .ToArray();
        var minValue = regions.Min();
        var maxValue = regions.Max();

        brokenOrder.RegionId.Should().NotBeInRange(minValue, maxValue);
    }

    private static void AssertEmptyItems(Order brokenOrder) => brokenOrder.Items.Should().BeEmpty();

    private static void AssertInvalidQuantity(Order brokenOrder)
    {
        brokenOrder.Items.Should().OnlyContain(item => item.Quantity < 0);
    }
}
