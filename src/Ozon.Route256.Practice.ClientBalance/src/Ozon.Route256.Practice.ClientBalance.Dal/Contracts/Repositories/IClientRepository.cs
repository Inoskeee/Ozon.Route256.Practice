using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;

public interface IClientRepository
{
    Task<long?> Insert(
        ClientEntity client,
        CancellationToken token);
}