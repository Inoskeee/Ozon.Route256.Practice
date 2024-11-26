using Dapper;

using Ozon.Route256.CustomerService.Domain;
using Ozon.Route256.CustomerService.Infrastructure.PostgresRepositories.Contracts;
using Ozon.Route256.CustomerService.Repositories;
using Ozon.Route256.CustomerService.Repositories.Exceptions;

namespace Ozon.Route256.CustomerService.Infrastructure.PostgresRepositories;

public class RegionRepository : BaseRepository, IRegionRepository
{
    public async Task<Region> Get(long regionId, CancellationToken token)
    {
        const string sqlQuery = @"
                SELECT id, name
                FROM regions 
                WHERE id = @RegionId;";

        var param = new DynamicParameters();
        param.Add("RegionId", regionId);

        var record = (await ExecuteQueryAsync<RegionRecord>(sqlQuery, param, token)).FirstOrDefault();

        if (record is null)
        {
            throw new RegionNotFoundException($"Invalid region id. The region with the '{regionId}' id is not found.");
        }

        return new Region(record.Id, record.Name);
    }
}