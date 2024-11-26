using Moq;
using Ozon.Route256.OrderService.Bll.Helpers.Interfaces;
using Ozon.Route256.OrderService.Bll.Services.Interfaces;
using Ozon.Route256.OrderService.Dal.Interfaces;
using Ozon.Route256.OrderService.Dal.Repositories;
using Ozon.Route256.OrderService.Kafka;

namespace Ozon.Route256.OrderService.UnitTests;

public class Mocks
{
    public readonly Mock<IOrdersRepository> OrdersRepositoryMock = new();
    public readonly Mock<IItemsRepository> ItemsRepositoryMock = new();
    public readonly Mock<IInputValidationHelper> InputValidatorHelperMock = new();
    public readonly Mock<ILogsService> LogsServiceMock = new();
    public readonly Mock<IOrderInputErrorsPublisher> OrderInputErrorsPublisherMock = new();
    public readonly Mock<IOrderOutputEventPublisher> OrderOutputEventPublisherMock = new();
}