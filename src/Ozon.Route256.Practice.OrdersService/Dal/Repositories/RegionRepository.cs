using Dapper;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Interfaces;

namespace Ozon.Route256.OrderService.Dal.Repositories;

public class RegionRepository : BaseRepository, IRegionRepository
{
    public RegionRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<RegionEntity?> Get(
        long regionId,
        CancellationToken token)
    {
        const string sqlQuery = @"
                select 
                    id,
                    name
                from 
                    regions 
                where 
                    id = @RegionId;";

        var param = new DynamicParameters();
        param.Add("RegionId", regionId);
        
        var result = await ExecuteQueryAsync<RegionEntity>(sqlQuery, param, token);
        return result.FirstOrDefault();
    }
}