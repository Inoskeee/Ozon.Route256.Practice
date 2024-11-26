using MediatR;
using Ozon.Route256.OrderService.Proto.Messages;
using Ozon.Route256.TestService.Common.Kafka.Consumer;
using Ozon.Route256.TestService.Data;
using Ozon.Route256.TestService.Domain.Actions.MatchCreatedOrder;

namespace Ozon.Route256.TestService.Integrations.Orders;

public class OrderOutputEventMessageProcessor : IMessageProcessor<string, OrderOutputEventMessage>
{
    private readonly IMismatchFeature _mismatchFeature;
    private readonly IMediator _mediator;

    public OrderOutputEventMessageProcessor(IMismatchFeature mismatchFeature, IMediator mediator)
    {
        _mismatchFeature = mismatchFeature;
        _mediator = mediator;
    }

    public Task ProcessMessageAsync(string key, OrderOutputEventMessage payload, CancellationToken cancellationToken)
    {
        if (!_mismatchFeature.IsEnabled)
            return Task.CompletedTask;

        var command = new MatchCreatedOrderCommand
        {
            Key = key,
            OrderId = payload.OrderId,
            EventType = payload.EventType.ToString()
        };

        return _mediator.Send(command, cancellationToken);
    }
}
