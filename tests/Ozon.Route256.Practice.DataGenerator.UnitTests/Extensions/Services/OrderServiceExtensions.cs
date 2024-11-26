using Moq;
using Ozon.Route256.DataGenerator.Bll.Models;
using Ozon.Route256.DataGenerator.Bll.Services.Contracts;

namespace Ozon.Route256.DataGenerator.UnitTests.Extensions.Services;

public static class OrderServiceExtensions
{
    public static Mock<IOrderService> SetupCreateQueue(
        this Mock<IOrderService> mock,
        IReadOnlyList<Customer> customers,
        Queue<Order> order)
    {
        mock.Setup(
                service => service.Create(customers))
            .Returns(order.Dequeue);

        return mock;
    }

    public static Mock<IOrderService> VerifyCreate(
        this Mock<IOrderService> mock,
        IReadOnlyList<Customer> customers,
        int times = 1)
    {
        mock.Verify(
            service => service.Create(customers),
            Times.Exactly(times));

        return mock;
    }
}
