using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Models;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Mappers;

public class ReportMapper : IReportMapper
{
    public ReportModel MapGrpcResponseToModel(V1QueryOrdersResponse response)
    {
        return new ReportModel(
            response.OrderId,
            response.Status.ToString(),
            response.Comment,
            response.CreatedAt.ToDateTimeOffset());
    }
}