using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Contracts;

public interface IClientBalanceUnitOfWork
{
    IClientRepository ClientRepository { get; }
    IClientBalanceRepository ClientBalanceRepository { get; }
    IOperationRepository<TopUpOperationEntity> TopUpOperationRepository { get; }
    IOperationRepository<WithdrawOperationEntity> WithdrawOperationRepository { get; }
}