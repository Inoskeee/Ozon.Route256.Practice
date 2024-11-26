using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.DataGenerator.Bll.Mediator.Commands;
using Ozon.Route256.DataGenerator.Bll.Services.Contracts;

namespace Ozon.Route256.DataGenerator.Infra.Workers;

public class OrdersGeneratorWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PeriodicTimer _periodicTimer;
    private readonly ILogger<OrdersGeneratorWorker> _logger;

    public OrdersGeneratorWorker(
        IServiceProvider serviceProvider,
        ILogger<OrdersGeneratorWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        var totalOrdersCount = 0L;

        while (await _periodicTimer.WaitForNextTickAsync(token))
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var brokenOrderService = scope.ServiceProvider.GetRequiredService<IBrokenOrderService>();
                var settings = scope.ServiceProvider.GetRequiredService<IOptions<OrdersGeneratorSettings>>();

                var brokenOrderIndexes = brokenOrderService.GetBrokenOrderIndexes(
                    totalOrdersCount,
                    settings.Value.InvalidOrderCounterNumber,
                    settings.Value.OrdersPerSecond);

                var command = new GenerateOrdersCommand
                {
                    OrdersCount = settings.Value.OrdersPerSecond,
                    CustomersCount = settings.Value.CustomersPerSecond,
                    BrokenOrdersIndexes = brokenOrderIndexes
                };

                var createdOrdersCount = await mediator.Send(command, token);

                totalOrdersCount += createdOrdersCount;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Ошибка при создании заказа: {Message}",
                    e.Message);
            }
        }
    }
}
