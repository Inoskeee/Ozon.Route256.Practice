using Dapper;
using Ozon.Route256.OrderService.Dal.Entities;
using Ozon.Route256.OrderService.Dal.Interfaces;

namespace Ozon.Route256.OrderService.Dal.Repositories;

public class LogsRepository : BaseRepository, ILogsRepository
{
    private const string OrderLogsTableFields = "order_id, region_id, status, customer_id, comment, created_at, updated_at";
    
    public LogsRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task Insert(
        OrderLogEntity logEntity,
        CancellationToken token)
    {
        const string sqlQuery = $@"
                insert into 
                    logs ({OrderLogsTableFields})
                values 
                    (@OrderId, @RegionId, @Status, @CustomerId, @Comment, @CreatedAt, @UpdatedAt);";

        var param = new DynamicParameters();
        param.Add("OrderId", logEntity.OrderId);
        param.Add("RegionId", logEntity.RegionId);
        param.Add("Status", logEntity.Status);
        param.Add("CustomerId", logEntity.CustomerId);
        param.Add("Comment", logEntity.Comment);
        param.Add("CreatedAt", logEntity.CreatedAt);
        param.Add("UpdatedAt", logEntity.UpdatedAt);

        await ExecuteNonQueryAsync(sqlQuery, param, token);
    }
}