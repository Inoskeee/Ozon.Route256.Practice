using Moq;
using Ozon.Route256.DataGenerator.Bll.Models;
using Ozon.Route256.DataGenerator.Bll.ProviderContracts;

namespace Ozon.Route256.DataGenerator.UnitTests.Extensions.Providers;

public static class CustomerProviderExtensions
{
    public static Mock<ICustomerProvider> SetupCreateCustomer(
        this Mock<ICustomerProvider> mock,
        Customer customer,
        long? customerId)
    {
        mock.Setup(
            provider => provider.CreateCustomer(
                customer,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(customerId);

        return mock;
    }

    public static Mock<ICustomerProvider> VerifyCreateCustomer(
        this Mock<ICustomerProvider> mock,
        Customer customer,
        int times = 1)
    {
        mock.Verify(
                provider => provider.CreateCustomer(
                    customer,
                    It.IsAny<CancellationToken>()),
                Times.Exactly(times));

        return mock;
    }
}
