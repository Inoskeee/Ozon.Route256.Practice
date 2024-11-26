using System.Text;
using Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Models;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Services;

public interface IReportFormatter
{
    Task<StringBuilder> FormatReport(List<ReportModel> reportModel);
}