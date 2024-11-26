using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace Ozon.Route256.TestService.Common.Kafka.Consumer;

[UsedImplicitly]
public class ConsumerBackgroundService<TKey, TValue> : BackgroundService
    where TKey : class
    where TValue : class
{
    private readonly IConsumerJob<TKey, TValue> _consumerJob;

    public ConsumerBackgroundService(IConsumerJob<TKey, TValue> consumerJob)
    {
        _consumerJob = consumerJob;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await _consumerJob.ConsumeAsync(stoppingToken);
    }
}
