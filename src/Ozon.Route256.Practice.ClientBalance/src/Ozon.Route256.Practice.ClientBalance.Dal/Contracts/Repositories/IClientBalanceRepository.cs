using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;

public interface IClientBalanceRepository
{
    Task<ClientBalanceEntity[]> Get(
        long clientId,
        CancellationToken token);
    
    Task<ResultEntity<long?>> UpdateOrInsert(
        ClientBalanceEntity balance,
        bool isTopUp,
        CancellationToken token);
    
}