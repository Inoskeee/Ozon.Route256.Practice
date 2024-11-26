using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;
using Ozon.Route256.Practice.OrdersReport.Bll.Configuration;
using Ozon.Route256.Practice.OrdersReport.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Services;

public class OrderGrpcClientProvider : IOrderGrpcClientProvider
{
    private readonly GrpcClientPackageConfig _clientPackageConfig;
    private readonly ILogger<OrderGrpcClientProvider> _logger;
    private readonly OrderGrpc.OrderGrpcClient _orderGrpcClient;

    public OrderGrpcClientProvider(
        ILogger<OrderGrpcClientProvider> logger,
        OrderGrpc.OrderGrpcClient orderGrpcClient,
        IOptions<GrpcClientPackageConfig> clientPackageOptions)
    {
        _logger = logger;
        _orderGrpcClient = orderGrpcClient;
        _clientPackageConfig = clientPackageOptions.Value;
    }

    public async Task<List<V1QueryOrdersResponse>> GetOrdersByCustomerAsync(
        long customerId, CancellationToken cancellationToken)
    {
        var orders = new List<V1QueryOrdersResponse>();

        var limit = _clientPackageConfig.Limit;
        var offset = 0;

        var isContinueRequest = true;

        while (isContinueRequest)
        {
            var request = new V1QueryOrdersRequest
            {
                CustomerIds = { customerId },
                Limit = limit,
                Offset = offset
            };

            var responseStream = _orderGrpcClient.V1QueryOrders(request);

            var receivedOrdersCount = 0;

            while (await responseStream.ResponseStream.MoveNext(cancellationToken))
            {
                var currentOrder = responseStream.ResponseStream.Current;
                orders.Add(currentOrder);
                receivedOrdersCount++;
            }

            _logger.LogInformation("Получено: {count} заказов со смещением {offset} для клиента {customerId}...",
                receivedOrdersCount, offset, customerId);


            if (receivedOrdersCount < limit)
                isContinueRequest = false;
            else
                offset += limit;
        }

        _logger.LogInformation("Всего получено: {count} заказов для клиента {customerId}",
            orders.Count, customerId);

        return orders;
    }
}