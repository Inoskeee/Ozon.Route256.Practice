using Dapper;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Repositories;

public sealed class TopUpOperationRespository : BaseRepository, IOperationRepository<TopUpOperationEntity>
{
    public async Task<TopUpOperationEntity?> TryGet(Guid guid, CancellationToken token)
    {
        const string sqlQuery = @"
                select *
                from top_up_operations
                where guid = @Guid";

        var param = new DynamicParameters();
        param.Add("Guid", guid);
        
        return (await ExecuteQueryAsync<TopUpOperationEntity>(sqlQuery, param, token)).FirstOrDefault(); 
    }

    public async Task<ResultEntity<string?>> Insert(TopUpOperationEntity operation, CancellationToken token)
    {
        var existingOperation = await TryGet(operation.Guid, token);
        if (existingOperation is not null)
        {
            return new ResultEntity<string?>(
                IsSuccess: false,
                Result: null,
                Message: $"Операция с guid {operation.Guid} уже существует");
        }
        
        const string sqlQuery = @"
                insert into top_up_operations(
                                              guid, 
                                              client_id, 
                                              currency_code, 
                                              units, 
                                              nanos,
                                              operation_type,
                                              operation_status,
                                              operation_time)
                values (@Guid,
                        @ClientId, 
                        @CurrencyCode, 
                        @Units, 
                        @Nanos,
                        @OperationType,
                        @OperationStatus,
                        @OperationTime)
                returning guid";

        var param = new DynamicParameters();
        param.Add("Guid", operation.Guid);
        param.Add("ClientId", operation.ClientId);
        param.Add("CurrencyCode", operation.CurrencyCode);
        param.Add("Units", operation.Units);
        param.Add("Nanos", operation.Nanos);
        param.Add("OperationType", operation.OperationTypeEntity);
        param.Add("OperationStatus", operation.OperationStatus);
        param.Add("OperationTime", operation.OperationTime);
        
        var queryResult = (await ExecuteNonQueryAsync<Guid?>(sqlQuery, param, token)).ToString();
        
        return !string.IsNullOrEmpty(queryResult) ? 
            new ResultEntity<string?>(
                IsSuccess: true,
                Result: queryResult,
                Message: "Операция упешно выполнена.")
            : new ResultEntity<string?>(
                IsSuccess: false,
                Result: queryResult,
                Message: "Не удалось выполнить запрос");
    }

    public async Task<ResultEntity<string?>> Update(TopUpOperationEntity operation, CancellationToken token)
    {
        const string sqlQuery = @"
                update top_up_operations
                set operation_status = @OperationStatus,
                    operation_time = @OperationTime
                where guid = @Guid
                returning guid";

        var param = new DynamicParameters();
        param.Add("Guid", operation.Guid);
        param.Add("OperationStatus", operation.OperationStatus);
        param.Add("OperationTime", operation.OperationTime);
        
        var queryResult = (await ExecuteNonQueryAsync<Guid?>(sqlQuery, param, token)).ToString();
        
        return !string.IsNullOrEmpty(queryResult) ? 
            new ResultEntity<string?>(
                IsSuccess: true,
                Result: queryResult,
                Message: "Операция упешно выполнена.")
            : new ResultEntity<string?>(
                IsSuccess: false,
                Result: queryResult,
                Message: "Не удалось выполнить запрос");
    }

}