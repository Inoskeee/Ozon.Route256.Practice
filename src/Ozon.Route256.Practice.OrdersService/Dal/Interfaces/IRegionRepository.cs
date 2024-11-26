using Ozon.Route256.OrderService.Dal.Entities;

namespace Ozon.Route256.OrderService.Dal.Interfaces;

public interface IRegionRepository
{
    Task<RegionEntity?> Get(
        long regionId,
        CancellationToken token);
}