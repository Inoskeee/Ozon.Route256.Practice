using Ozon.Route256.Practice.OrdersFacade.OrderGrpc;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Models;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Mappers;

public interface IReportMapper
{
    ReportModel MapGrpcResponseToModel(V1QueryOrdersResponse response);
}