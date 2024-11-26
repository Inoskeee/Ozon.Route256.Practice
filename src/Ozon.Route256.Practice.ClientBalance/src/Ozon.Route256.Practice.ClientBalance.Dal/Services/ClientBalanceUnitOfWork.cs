using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Services;

public class ClientBalanceUnitOfWork : IClientBalanceUnitOfWork
{
    public IClientRepository ClientRepository { get; }
    public IClientBalanceRepository ClientBalanceRepository { get; }
    public IOperationRepository<TopUpOperationEntity> TopUpOperationRepository { get; }
    public IOperationRepository<WithdrawOperationEntity> WithdrawOperationRepository { get; }
    
    public ClientBalanceUnitOfWork(
        IClientRepository clientRepository,
        IClientBalanceRepository clientBalanceRepository,
        IOperationRepository<TopUpOperationEntity> topUpOperationRepository,
        IOperationRepository<WithdrawOperationEntity> withdrawOperationRepository)
    {
        ClientRepository = clientRepository;
        ClientBalanceRepository = clientBalanceRepository;
        TopUpOperationRepository = topUpOperationRepository;
        WithdrawOperationRepository = withdrawOperationRepository;
    }
}