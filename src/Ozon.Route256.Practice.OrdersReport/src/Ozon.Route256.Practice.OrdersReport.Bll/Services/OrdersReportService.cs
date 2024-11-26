using System.Text;
using Microsoft.Extensions.Logging;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Models;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Services;
using Ozon.Route256.Practice.OrdersReport.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Services;

public class OrdersReportService : IOrdersReportService
{
    private readonly ILogger<OrdersReportService> _logger;
    private readonly IOrderGrpcClientProvider _orderGrpcClient;

    private readonly IReportMapper _reportMapper;

    public OrdersReportService(
        ILogger<OrdersReportService> logger,
        IOrderGrpcClientProvider orderGrpcClient,
        IReportMapper reportMapper)
    {
        _logger = logger;
        _orderGrpcClient = orderGrpcClient;
        _reportMapper = reportMapper;
    }

    public async Task<StringBuilder> GetReportByCustomer(
        long customerId, IReportFormatter reportFormatter, CancellationToken cancellationToken)
    {
        var ordersByCustomer = await _orderGrpcClient.GetOrdersByCustomerAsync(
            customerId,
            cancellationToken);

        var ordersModelList = new List<ReportModel>();

        foreach (var order in ordersByCustomer) ordersModelList.Add(_reportMapper.MapGrpcResponseToModel(order));

        var report = await reportFormatter.FormatReport(ordersModelList);

        return report;
    }
}