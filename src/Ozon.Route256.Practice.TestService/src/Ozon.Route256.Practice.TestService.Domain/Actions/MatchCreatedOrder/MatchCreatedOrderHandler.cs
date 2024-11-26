using MediatR;
using Microsoft.Extensions.Logging;
using Ozon.Route256.TestService.Data;
using Ozon.Route256.TestService.Data.Customers;
using Ozon.Route256.TestService.Data.Orders;
using Ozon.Route256.TestService.Domain.Metrics;

namespace Ozon.Route256.TestService.Domain.Actions.MatchCreatedOrder;

public class MatchCreatedOrderHandler : IRequestHandler<MatchCreatedOrderCommand>
{
    private static readonly object Empty = new();

    private readonly IOrdersRepository _ordersRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly IMismatchRepository _mismatchRepository;
    private readonly IMismatchMetricReporter _mismatchMetricReporter;
    private readonly ILogger<MatchCreatedOrderHandler> _logger;

    public MatchCreatedOrderHandler(
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository,
        IMismatchRepository mismatchRepository,
        IMismatchMetricReporter mismatchMetricReporter,
        ILogger<MatchCreatedOrderHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _customersRepository = customersRepository;
        _mismatchRepository = mismatchRepository;
        _mismatchMetricReporter = mismatchMetricReporter;
        _logger = logger;
    }

    public async Task Handle(MatchCreatedOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _ordersRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);

        if (order == null)
        {
            AddOrderNotExistMismatch(request);

            return;
        }

        _logger.LogInformation("Order found. OrderId: {OrderId}", request.OrderId);

        var customer = await _customersRepository.GetCustomerByIdAsync(order.CustomerId, cancellationToken);

        if (customer == null)
        {
            AddCustomerNotExistMismatch(request, order);

            return;
        }

        _logger.LogInformation(
            "Customer found for order. OrderId: {OrderId}, CustomerId: {CustomerId}",
            request.OrderId,
            customer.Id);

        _mismatchMetricReporter.Inc(MismatchType.None);
    }

    private void AddOrderNotExistMismatch(MatchCreatedOrderCommand request)
    {
        var mismatch = new Mismatch
        {
            Key = request.Key,
            Type = MismatchType.OrderNotExist,
            Payload = new Payload
            {
                OrderId = request.OrderId,
                EventType = request.EventType
            },
            StoredData = Empty
        };

        _logger.LogInformation("Mismatch detected. OrderId: {OrderId}, Mismatch: {MismatchType}", request.OrderId, mismatch.Type);

        _mismatchRepository.AddMismatch(mismatch);

        _mismatchMetricReporter.Inc(mismatch.Type);
    }

    private void AddCustomerNotExistMismatch(MatchCreatedOrderCommand request, Order order)
    {
        var mismatch = new Mismatch
        {
            Key = request.Key,
            Type = MismatchType.CustomerNotExist,
            Payload = new Payload
            {
                OrderId = request.OrderId,
                EventType = request.EventType
            },
            StoredData = order
        };

        _logger.LogInformation("Mismatch detected. OrderId: {OrderId}, Mismatch: {MismatchType}", request.OrderId, mismatch.Type);

        _mismatchRepository.AddMismatch(mismatch);

        _mismatchMetricReporter.Inc(mismatch.Type);
    }
}
