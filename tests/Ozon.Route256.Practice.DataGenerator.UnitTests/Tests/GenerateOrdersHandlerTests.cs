using FluentAssertions;
using Ozon.Route256.DataGenerator.Bll.Creators;
using Ozon.Route256.DataGenerator.Bll.Mapping;
using Ozon.Route256.DataGenerator.Bll.Mediator.Commands;
using Ozon.Route256.DataGenerator.Bll.Models;
using Ozon.Route256.DataGenerator.UnitTests.Extensions.Providers;
using Ozon.Route256.DataGenerator.UnitTests.Extensions.Services;
using Ozon.Route256.DataGenerator.UnitTests.Stubs;
using Random = Ozon.Route256.DataGenerator.Bll.Creators.Random;

namespace Ozon.Route256.DataGenerator.UnitTests.Tests;

public class GenerateOrdersHandlerTests
{
    [Fact]
    public async Task Orders_ShouldNotBeGenerated_WhenCustomerCount_WasNotConfigured()
    {
        // arrange
        var command = new GenerateOrdersCommand
        {
            CustomersCount = 0,
            OrdersCount = 10,
            BrokenOrdersIndexes = []
        };
        var handler = StubFactory.CreateGenerateOrdersHandlerStub();

        // act
        var createdOrdersCount = await handler.Handle(command, token: default);

        // assert
        createdOrdersCount.Should().Be(0);

        handler.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Orders_ShouldNotBeGenerated_WhenNoCustomersCreated()
    {
        // arrange
        var command = new GenerateOrdersCommand
        {
            CustomersCount = 2,
            OrdersCount = 2,
            BrokenOrdersIndexes = []
        };
        var customers = CustomerCreator.Create(command.CustomersCount);
        var handler = StubFactory.CreateGenerateOrdersHandlerStub();
        handler.CustomerService
            .SetupCreate(command.CustomersCount, customers);
        handler.CustomerProvider
            .SetupCreateCustomer(customers[0], null)
            .SetupCreateCustomer(customers[1], null);

        // act
        var createdOrdersCount = await handler.Handle(command, token: default);

        // assert
        createdOrdersCount.Should().Be(0);

        handler.CustomerService
            .VerifyCreate(command.CustomersCount);
        handler.CustomerProvider
            .VerifyCreateCustomer(customers[0])
            .VerifyCreateCustomer(customers[1]);

        handler.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Orders_ShouldNotBeGenerated_WhenOrdersCount_WasNotConfigured()
    {
        // arrange
        var command = new GenerateOrdersCommand
        {
            CustomersCount = 1,
            OrdersCount = 0,
            BrokenOrdersIndexes = []
        };
        var customers = CustomerCreator.Create(command.CustomersCount);
        var handler = StubFactory.CreateGenerateOrdersHandlerStub();
        handler.CustomerService
            .SetupCreate(command.CustomersCount, customers);
        handler.CustomerProvider
            .SetupCreateCustomer(customers[0], Random.RandomId);

        // act
        var createdOrdersCount = await handler.Handle(command, token: default);

        // assert
        createdOrdersCount.Should().Be(0);

        handler.CustomerService
            .VerifyCreate(command.CustomersCount);
        handler.CustomerProvider
            .VerifyCreateCustomer(customers[0]);

        handler.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Order_WasGenerated_Correctly()
    {
        // arrange
        var command = new GenerateOrdersCommand
        {
            CustomersCount = 1,
            OrdersCount = 2,
            BrokenOrdersIndexes = []
        };
        var customer = CustomerCreator.Create().Single();
        var customers = new[] { customer };
        var identifiedCustomer = customer with { Id = Random.RandomId };
        var identifiedCustomers = new[] { identifiedCustomer };
        var firstOrder = OrderCreator.Create() with
        {
            CustomerId = identifiedCustomer.Id!.Value,
            RegionId = identifiedCustomer.RegionId,
            Items = ItemCreator.Create(Random.ItemsCount)
        };
        var secondOrder = OrderCreator.Create() with
        {
            CustomerId = identifiedCustomer.Id!.Value,
            RegionId = identifiedCustomer.RegionId,
            Items = ItemCreator.Create(Random.ItemsCount)
        };
        var ordersQueue = new Queue<Order>();
        ordersQueue.Enqueue(firstOrder);
        ordersQueue.Enqueue(secondOrder);

        var handler = StubFactory.CreateGenerateOrdersHandlerStub();
        handler.CustomerService
            .SetupCreate(command.CustomersCount, customers);
        handler.CustomerProvider
            .SetupCreateCustomer(customer, identifiedCustomer.Id);
        handler.OrderService
            .SetupCreateQueue(identifiedCustomers, ordersQueue);

        // act
        var createdOrdersCount = await handler.Handle(command, token: default);

        // assert
        createdOrdersCount.Should().Be(command.OrdersCount);

        handler.CustomerService
            .VerifyCreate(command.CustomersCount);
        handler.CustomerProvider
            .VerifyCreateCustomer(customers[0]);
        handler.OrderService
            .VerifyCreate(identifiedCustomers, times: 2);
        handler.KafkaProvider
            .VerifyPublish(firstOrder.Map())
            .VerifyPublish(secondOrder.Map());

        handler.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Order_And_BrokenOrder_WasGenerated_Correctly()
    {
        // arrange
        var command = new GenerateOrdersCommand
        {
            CustomersCount = 1,
            OrdersCount = 2,
            BrokenOrdersIndexes = [0]
        };
        var customer = CustomerCreator.Create().Single();
        var customers = new[] { customer };
        var identifiedCustomer = customer with { Id = Random.RandomId };
        var identifiedCustomers = new[] { identifiedCustomer };
        var firstOrder = OrderCreator.Create() with
        {
            CustomerId = identifiedCustomer.Id!.Value,
            RegionId = identifiedCustomer.RegionId,
            Items = ItemCreator.Create(Random.ItemsCount)
        };
        var secondOrder = OrderCreator.Create() with
        {
            CustomerId = identifiedCustomer.Id!.Value,
            RegionId = identifiedCustomer.RegionId,
            Items = ItemCreator.Create(Random.ItemsCount)
        };
        var brokenOrder = firstOrder with { Items = [] };
        var ordersQueue = new Queue<Order>();
        ordersQueue.Enqueue(firstOrder);
        ordersQueue.Enqueue(secondOrder);

        var handler = StubFactory.CreateGenerateOrdersHandlerStub();
        handler.CustomerService
            .SetupCreate(command.CustomersCount, customers);
        handler.CustomerProvider
            .SetupCreateCustomer(customer, identifiedCustomer.Id);
        handler.OrderService
            .SetupCreateQueue(identifiedCustomers, ordersQueue);
        handler.BrokenOrderService
            .SetupBreakOrder(firstOrder, brokenOrder);

        // act
        var createdOrdersCount = await handler.Handle(command, token: default);

        // assert
        createdOrdersCount.Should().Be(command.OrdersCount);

        handler.CustomerService
            .VerifyCreate(command.CustomersCount);
        handler.CustomerProvider
            .VerifyCreateCustomer(customers[0]);
        handler.OrderService
            .VerifyCreate(identifiedCustomers, times: 2);
        handler.KafkaProvider
            .VerifyPublish(brokenOrder.Map())
            .VerifyPublish(secondOrder.Map());
        handler.BrokenOrderService
            .VerifyBreakOrder(firstOrder);

        handler.VerifyNoOtherCalls();
    }
}
