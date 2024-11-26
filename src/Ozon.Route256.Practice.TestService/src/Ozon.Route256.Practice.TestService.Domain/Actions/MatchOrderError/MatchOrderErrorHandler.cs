using MediatR;
using Microsoft.Extensions.Logging;
using Ozon.Route256.TestService.Data;
using Ozon.Route256.TestService.Data.Orders;
using Ozon.Route256.TestService.Domain.Actions.MatchCreatedOrder;
using Ozon.Route256.TestService.Domain.Metrics;

namespace Ozon.Route256.TestService.Domain.Actions.MatchOrderError;

public class MatchOrderErrorHandler : IRequestHandler<MatchOrderErrorCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMismatchRepository _mismatchRepository;
    private readonly IMismatchMetricReporter _mismatchMetricReporter;
    private readonly ILogger<MatchCreatedOrderHandler> _logger;

    public MatchOrderErrorHandler(
        IOrdersRepository ordersRepository,
        IMismatchRepository mismatchRepository,
        IMismatchMetricReporter mismatchMetricReporter,
        ILogger<MatchCreatedOrderHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _mismatchRepository = mismatchRepository;
        _mismatchMetricReporter = mismatchMetricReporter;
        _logger = logger;
    }

    public async Task Handle(MatchOrderErrorCommand request, CancellationToken cancellationToken)
    {
        var orders = await SearchOrdersAsync(request, cancellationToken);

        if (orders.Count > 0)
        {
            AddOrderCreatedOnErrorMismatch(request, orders);

            return;
        }

        _logger.LogInformation(
            "Orders not found on error. CustomerId: {CustomerId}, RegionId: {RegionId}",
            request.CustomerId,
            request.RegionId);

        _mismatchMetricReporter.Inc(MismatchType.None);
    }

    private Task<IReadOnlyCollection<Order>> SearchOrdersAsync(MatchOrderErrorCommand request, CancellationToken cancellationToken)
    {
        var searchParams = new SearchOrdersQueryParams
        {
            CustomerId = request.CustomerId,
            RegionId = request.RegionId,
            Items = request.OrderItems
                .Select(item => new SearchOrdersQueryParams.SearchOrdersItem
                {
                    Barcode = item.Barcode,
                    Quantity = item.Quantity
                })
                .ToArray()
        };

        return _ordersRepository.SearchOrdersAsync(searchParams, cancellationToken);
    }

    private void AddOrderCreatedOnErrorMismatch(MatchOrderErrorCommand request, IReadOnlyCollection<Order> orders)
    {
        var mismatch = new Mismatch
        {
            Key = request.Key,
            Type = MismatchType.OrderCreatedOnError,
            Payload = new Payload
            {
                CustomerId = request.CustomerId,
                OrderItems = request.OrderItems,
                RegionId = request.RegionId
            },
            StoredData = new StoredData
            {
                Orders = orders
            }
        };

        _logger.LogInformation(
            "Mismatch detected. CustomerId: {OrderId}, RegionId: {RegionId}, Mismatch: {MismatchType}",
            request.CustomerId,
            request.RegionId,
            mismatch.Type);

        _mismatchRepository.AddMismatch(mismatch);

        _mismatchMetricReporter.Inc(mismatch.Type);
    }
}
