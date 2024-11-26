using Ozon.Route256.OrderService.Dal.Entities;

namespace Ozon.Route256.OrderService.Dal.Interfaces;

public interface ILogsRepository
{
    Task Insert(
        OrderLogEntity logEntity,
        CancellationToken token);
}