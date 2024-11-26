using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Contracts;

public interface IBalanceOperationsDbProvider
{
    Task<ResultEntity<string?>> CreateTopUpOperation(
        TopUpOperationEntity operation,
        CancellationToken token);
    
    Task<ResultEntity<string?>> CreateWithdrawOperation(
        WithdrawOperationEntity operation,
        CancellationToken token);
    
    Task<ResultEntity<string?>> ChangeTopUpOperation(
        TopUpOperationEntity operation,
        CancellationToken token);
    
    Task<ResultEntity<string?>> ChangeWithdrawOperation(
        WithdrawOperationEntity operation,
        CancellationToken token);
}