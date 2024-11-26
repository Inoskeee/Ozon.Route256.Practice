using FluentAssertions;
using Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Creators;

namespace Ozon.Route256.Practice.ViewOrderService.IntegrationTests.Repositories;

public partial class OrdersRepositoryTests
{
    [Fact]
    public async Task GetOrderById_WhenAddNewStudent_RetunsJustCreatedStudent()
    {
        //Arrange
        var expected = OrderEntityCreator
            .Generate()
            .WithCreatedAt(DateTime.UtcNow);

        //Act
        await _repository.AddOrder(
            expected, 
            default);

        var result = await _repository.GetOrderById(
            expected.OrderId, 
            default);
        
        //Assert
        result?.CreatedAt.Should().BeCloseTo(expected.CreatedAt, precision: TimeSpan.FromMilliseconds(1));
    }
    
    [Fact]
    public async Task GetOrderById_WhenStudentNotFound_RetunsNull()
    {
        //Arrange
        var expected = OrderEntityCreator
            .Generate();

        //Act
        var result = await _repository.GetOrderById(
            expected.OrderId, 
            default);

        //Assert
        result.Should().BeNull();
    }
    
}