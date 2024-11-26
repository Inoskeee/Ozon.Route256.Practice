using Ozon.Route256.DataGenerator.Messages;

namespace Ozon.Route256.DataGenerator.Bll.ProviderContracts;

public interface IKafkaProvider
{
    public Task Publish(OrderInputMessage message, CancellationToken token);
}
