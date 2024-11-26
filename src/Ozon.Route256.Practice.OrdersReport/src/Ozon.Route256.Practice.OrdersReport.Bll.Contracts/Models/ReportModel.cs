namespace Ozon.Route256.Practice.OrdersReport.Bll.Contracts.Models;

public record ReportModel(
    long OrderId, 
    string OrderStatus, 
    string Comment, 
    DateTimeOffset DateCreated);