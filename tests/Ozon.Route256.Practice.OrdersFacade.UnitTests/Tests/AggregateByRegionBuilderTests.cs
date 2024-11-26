using AutoFixture.AutoNSubstitute;
using Ozon.Route256.Practice.OrdersFacade.Bll.Builders;
using Ozon.Route256.Practice.OrdersFacade.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities;
using Ozon.Route256.Practice.OrdersFacade.Bll.Entities.Enums;

namespace Ozon.Route256.Practice.OrdersFacade.UnitTests.Tests;

public class AggregateByRegionBuilderTests
{
    private readonly IFixture _fixture;

    private readonly AggregateByRegionBuilder _builder;
    private readonly IEntityMapper _entityMapper;

    public AggregateByRegionBuilderTests()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());;
        _entityMapper = Substitute.For<IEntityMapper>();
        _builder = new AggregateByRegionBuilder(_entityMapper);
    }

    [Fact]
    public void Build_ShouldReturn_CorrectAggregationByRegion()
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
            .With(o => o.Status, OrderStatusEnum.ORDER_STATUS_NEW)
            .Create();
        
        var customerEntity = _fixture.Build<CustomerEntity>()
            .With(c => c.CustomerId, 1)
            .With(c => c.FullName, "John Doe")
            .With(c => c.Region, regionEntity)
            .Create();

        var orderEntities = new List<OrderEntityWithCustomerId> { orderEntity };
        var customerEntities = new List<CustomerEntity> { customerEntity };

        var expectedRegion = _fixture.Build<RegionEntity>()
            .With(r => r.Id, 1)
            .With(r => r.Name, "Москва")
            .Create();
        
        var expectedMappedOrder = _fixture.Build<OrderEntityWithCustomerEntity>()
            .With(em => em.OrderId, 10)
            .With(em => em.Customer, customerEntity)
            .With(em => em.Comment, "Тестовый комментарий")
            .With(em => em.Region, regionEntity)
            .With(em => em.Status, OrderStatusEnum.ORDER_STATUS_NEW)
            .Create();

        // Act
        _entityMapper
            .MapCustomerToRegionOrderEntity(orderEntities[0], customerEntities[0])
            .Returns(expectedMappedOrder);
        var result = _builder.Build(orderEntities, customerEntities);

        // Assert
        Assert.Equal(expectedRegion.Id, result.Region.Id);
        Assert.Single(result.Orders);
        Assert.Contains(expectedMappedOrder, result.Orders);
    }

    [Fact]
    public void Build_ShouldReturn_EmptyOrders()
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
            .With(o => o.Status, OrderStatusEnum.ORDER_STATUS_NEW)
            .Create();

        var orderEntities = new List<OrderEntityWithCustomerId> { orderEntity };
        var customerEntities = new List<CustomerEntity>();
        
        // Act
        var result = _builder.Build(orderEntities, customerEntities);

        // Assert
        Assert.Empty(result.Orders);
    }
}