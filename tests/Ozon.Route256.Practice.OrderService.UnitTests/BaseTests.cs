using System.Linq.Expressions;
using Bogus;
using Moq;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Interfaces;
using Ozon.Route256.OrderService.Proto.OrderGrpc;

namespace Ozon.Route256.OrderService.UnitTests;

public class BaseTests
{
    private static Faker Faker = new();
    
    protected CancellationToken Token = CancellationToken.None;
    
    protected readonly Mocks Mocks = new();
    
    protected int RandomCount => Faker.Random.Int(1, 100);
    
    protected long RandomId => Faker.Random.Long(100000, 999999);

    private string RandomString => Faker.Lorem.Text();

    private OrderStatus RandomOrderStatus => Faker.Random.Enum(OrderStatus.Undefined);
    
    protected void SetupJobRepository<TResponse>(
        Expression<Func<IOrdersRepository, Task<TResponse>>> setup,
        TResponse response)
    {
        Mocks.OrdersRepositoryMock.Setup(setup).ReturnsAsync(response);
    }
    
    protected long[] GetRandomIds(int idCount)
    {
        return Faker.MakeLazy(idCount, () => RandomId).ToArray();
    }

    protected OrderInfoEntity[] GenerateOrderInfoEntities(
        IEnumerable<long> orderIds,
        OrderStatus? orderStatus = null)
    {
        return orderIds.Select(
                orderId => new OrderInfoEntity
                {
                    OrderId = orderId,
                    RegionId = RandomId,
                    CustomerId = RandomId,
                    RegionName = RandomString,
                    Comment = RandomString,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Status = orderStatus is null ? (int)RandomOrderStatus : (int)orderStatus,
                    TotalCount = orderIds.Count()
                })
            .ToArray();
    }
}