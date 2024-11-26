using FluentAssertions;
using Moq;
using Ozon.Route256.OrderService.Bll.Exceptions;
using Ozon.Route256.OrderService.Bll.Extensions;
using Ozon.Route256.OrderService.Bll.Models;
using Ozon.Route256.OrderService.Bll.Services.Interfaces;
using Ozon.Route256.OrderService.Dal.Models;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.UnitTests.Services;

public class OrderServiceTests : BaseTests
{
    private readonly IOrderService _orderService;

    public OrderServiceTests()
    {
        _orderService = new Bll.Services.OrderService(
            Mocks.OrdersRepositoryMock.Object,
            Mocks.LogsServiceMock.Object,
            Mocks.OrderInputErrorsPublisherMock.Object,
            Mocks.OrderOutputEventPublisherMock.Object,
            Mocks.InputValidatorHelperMock.Object,
            Mocks.ItemsRepositoryMock.Object);
    }

    [Fact]
    public async Task QueryOrders_Success()
    {
        //Arrange
        var orderCount = RandomCount;
        var orderIds = GetRandomIds(orderCount);
        var orderEntities = GenerateOrderInfoEntities(orderIds);
        var customerIds = Array.Empty<long>();
        var regionIds = Array.Empty<long>();
        var limit = RandomCount;
        var offset = RandomCount;

        SetupJobRepository(
            provider => provider.Query(It.IsAny<OrderQueryModel>(), Token),
            orderEntities);

        //Act
        var (orders, totalCount) = await _orderService.QueryOrders(orderIds, customerIds, regionIds, limit, offset, Token);
        
        //Assert
        orders.Should().NotBeNullOrEmpty();
        orders.Should().AllBeOfType<Order>();
        totalCount.Should().Be(orderCount);
        orders.Length.Should().Be(orderCount);
        orders.Select(order => order.OrderId).Should().BeEquivalentTo(orderIds);
        orders.Should()
            .AllSatisfy(
                order =>
                {
                    order.Status.Should()
                        .BeOneOf(
                            OrderStatus.New,
                            OrderStatus.Canceled,
                            OrderStatus.Delivered);
                });
    }

    [Fact]
    public async Task CancelOrder_Success()
    {
        //Arrange
        var orderId = RandomId;
        var orderEntities = GenerateOrderInfoEntities(
            orderIds: orderId.MakeArray(),
            orderStatus: OrderStatus.New);
        
        SetupJobRepository(
            provider => provider.Query(It.IsAny<OrderQueryModel>(), Token),
            orderEntities);
        
        //Act
        var result = await _orderService.CancelOrder(orderId, Token);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Result>();
        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task CancelOrder_ShouldReturn_OrderNotFoundException()
    {
        //Arrange
        var orderId = RandomId;

        //Act
        var result = await _orderService.CancelOrder(orderId, Token);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Result>();
        result.Success.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error?.ErrorCode.Should().Be(nameof(OrderNotFoundException));
        result.Error?.ErrorMessage.Should().Be($"Order {orderId} is not found");
    }
    
    [Fact]
    public async Task CancelOrder_ShouldReturn_InvalidOrderStatusException()
    {
        //Arrange
        var orderId = RandomId;
        var orderEntities = GenerateOrderInfoEntities(
            orderIds: orderId.MakeArray(),
            orderStatus: OrderStatus.Delivered);
        
        SetupJobRepository(
            provider => provider.Query(It.IsAny<OrderQueryModel>(), Token),
            orderEntities);

        //Act
        var result = await _orderService.CancelOrder(orderId, Token);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Result>();
        result.Success.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error?.ErrorCode.Should().Be(nameof(InvalidOrderStatusException));
        result.Error?.ErrorMessage.Should().Be("Invalid order status");
    }
    
    [Fact]
    public async Task DeliveryOrder_Success()
    {
        //Arrange
        var orderId = RandomId;
        var orderEntities = GenerateOrderInfoEntities(
            orderIds: orderId.MakeArray(),
            orderStatus: OrderStatus.New);
        
        SetupJobRepository(
            provider => provider.Query(It.IsAny<OrderQueryModel>(), Token),
            orderEntities);
        
        //Act
        var result = await _orderService.DeliveryOrder(orderId, Token);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Result>();
        result.Success.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task DeliveryOrder_ShouldReturn_OrderNotFoundException()
    {
        //Arrange
        var orderId = RandomId;

        //Act
        var result = await _orderService.DeliveryOrder(orderId, Token);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Result>();
        result.Success.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error?.ErrorCode.Should().Be(nameof(OrderNotFoundException));
        result.Error?.ErrorMessage.Should().Be($"Order {orderId} is not found");
    }
    
    [Fact]
    public async Task DeliveryOrder_ShouldReturn_InvalidOrderStatusException()
    {
        //Arrange
        var orderId = RandomId;
        var orderEntities = GenerateOrderInfoEntities(
            orderIds: orderId.MakeArray(),
            orderStatus: OrderStatus.Canceled);
        
        SetupJobRepository(
            provider => provider.Query(It.IsAny<OrderQueryModel>(), CancellationToken.None),
            orderEntities);

        //Act
        var result = await _orderService.DeliveryOrder(orderId, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Result>();
        result.Success.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error?.ErrorCode.Should().Be(nameof(InvalidOrderStatusException));
        result.Error?.ErrorMessage.Should().Be("Invalid order status");
    }
}