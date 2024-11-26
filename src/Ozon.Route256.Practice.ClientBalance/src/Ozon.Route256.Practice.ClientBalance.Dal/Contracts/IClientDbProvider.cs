using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Contracts;

public interface IClientDbProvider
{
    Task<long?> CreateClient(
        ClientEntity client,
        CancellationToken token);
    
    Task<ClientBalanceEntity[]> GetClientBalance(
        long clientId,
        CancellationToken token);
}