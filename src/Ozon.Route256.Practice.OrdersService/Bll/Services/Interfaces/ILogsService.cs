using Ozon.Route256.OrderService.Bll.Models;

namespace Ozon.Route256.OrderService.Bll.Services.Interfaces;

public interface ILogsService
{
    Task AddLog(
        Order order,
        CancellationToken token);
}