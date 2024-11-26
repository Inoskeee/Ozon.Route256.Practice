using AutoFixture.AutoNSubstitute;
using Ozon.Route256.Practice.OrdersFacade.Bll.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.UnitTests.Tests;

public class AggregateByCustomerBuilderTests
{
    private readonly IFixture _fixture;
    private readonly AggregateByCustomerBuilder _builder;
    
    public AggregateByCustomerBuilderTests()
    { 
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _builder = new AggregateByCustomerBuilder();
    }
    
    [Fact]
    public void Build_ShouldReturn_CorrectAggregationByCustomer()
    {
        // Arrange
        var regionEntity = _fixture.Build<RegionEntity>()
            .With(r => r.Id, 1)
            .With(r => r.Name, "Москва")
            .Create();
        
        var orderEntity = _fixture.Build<OrderEntityWithCustomerId>()
            .With(o => o.CustomerId, 1)
            .With(o => o.OrderId, 10)
            .With(o => o.Region, regionEntity)
            .With(o => o.Comment, "Тестовый комментарий")
            .With(o => o.CreatedAt, new DateTimeOffset(new DateTime(2024, 09, 14)))
            .With(o => o.Status, OrderStatusEnum.ORDER_STATUS_NEW)
            .Create();
        
        var customerEntity = _fixture.Build<CustomerEntity>()
            .With(c => c.CustomerId, 1)
            .With(c => c.FullName, "John Doe")
            .With(c => c.Region, regionEntity)
            .Create();

        var orderEntities = new List<OrderEntityWithCustomerId> { orderEntity };
        var customerEntities = new List<CustomerEntity> { customerEntity };

        var expectedCustomer = _fixture.Build<CustomerEntity>()
            .With(c => c.CustomerId, 1)
            .With(c => c.FullName, "John Doe")
            .With(c => c.Region, regionEntity)
            .Create();
        
        var expectedMappedOrder = _fixture.Build<OrderEntityWithCustomerId>()
            .With(em => em.OrderId, 10)
            .With(em => em.CustomerId, expectedCustomer.CustomerId)
            .With(em => em.Comment, "Тестовый комментарий")
            .With(em => em.Region, regionEntity)
            .With(o => o.CreatedAt, new DateTimeOffset(new DateTime(2024, 09, 14)))
            .With(em => em.Status, OrderStatusEnum.ORDER_STATUS_NEW)
            .Create();

        // Act
        var result = _builder.Build(orderEntities, customerEntities);

        // Assert
        Assert.Equal(expectedCustomer.CustomerId, result.Customer.CustomerId);
        Assert.Single(result.Orders);
        Assert.Contains(expectedMappedOrder.CustomerId, result.Orders.Select(x=>x.CustomerId));
        Assert.Contains(expectedMappedOrder.OrderId, result.Orders.Select(x=>x.OrderId));
    }
    
    [Fact]
    public void Build_ShouldReturn_EmptyOrders()
    {
        // Arrange
        var orderEntities = new List<OrderEntityWithCustomerId>();
        var customerEntities = new List<CustomerEntity>();
        
        // Act
        var result = _builder.Build(orderEntities, customerEntities);

        // Assert
        Assert.Empty(result.Orders);
    }
}