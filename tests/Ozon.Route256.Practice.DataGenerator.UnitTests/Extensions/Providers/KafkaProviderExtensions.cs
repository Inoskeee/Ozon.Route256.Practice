using Moq;
using Ozon.Route256.DataGenerator.Bll.ProviderContracts;
using Ozon.Route256.DataGenerator.Messages;

namespace Ozon.Route256.DataGenerator.UnitTests.Extensions.Providers;

public static class KafkaProviderExtensions
{
    public static Mock<IKafkaProvider> VerifyPublish(
        this Mock<IKafkaProvider> mock,
        OrderInputMessage expected,
        int times = 1)
    {
        mock.Verify(
            provider => provider.Publish(
                It.Is<OrderInputMessage>(actual => actual.JsonCompare(expected)),
                It.IsAny<CancellationToken>()),
            Times.Exactly(times));

        return mock;
    }
}
