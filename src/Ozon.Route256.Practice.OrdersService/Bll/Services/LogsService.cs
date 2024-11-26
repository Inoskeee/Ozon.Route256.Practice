using Ozon.Route256.OrderService.Bll.Models;
using Ozon.Route256.OrderService.Bll.Services.Interfaces;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Interfaces;

namespace Ozon.Route256.OrderService.Bll.Services;

public class LogsService : ILogsService
{
    private readonly ILogsRepository _logsRepository;

    public LogsService(
        ILogsRepository logsRepository)
    {
        _logsRepository = logsRepository;
    }

    public async Task AddLog(
        Order order, 
        CancellationToken token)
    {
        await _logsRepository.Insert(
            new OrderLogEntity
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                RegionId = order.Region.Id,
                Status = (int)order.Status,
                Comment = order.Comment,
                CreatedAt = order.CreatedAt,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            token);
    }
}