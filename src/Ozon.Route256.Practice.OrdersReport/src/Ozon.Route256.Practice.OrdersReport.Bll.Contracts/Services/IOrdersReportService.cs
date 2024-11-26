
using System.Text;
using Ozon.Route256.Practice.OrdersReport.Bll.Services.Contracts;

namespace Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Services;

public interface IOrdersReportService
{
    Task<StringBuilder> GetReportByCustomer(
        long customerId, 
        IReportFormatter reportFormatter, 
        CancellationToken cancellationToken);
}