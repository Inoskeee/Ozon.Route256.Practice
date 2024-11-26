using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Services;

public class ClientDbProvider : IClientDbProvider
{
    private readonly IClientBalanceUnitOfWork _unitOfWork;

    public ClientDbProvider(
        IClientBalanceUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long?> CreateClient(ClientEntity client, CancellationToken token)
    {
        return await _unitOfWork.ClientRepository.Insert(client, token);
    }

    public async Task<ClientBalanceEntity[]> GetClientBalance(long clientId, CancellationToken token)
    {
        return await _unitOfWork.ClientBalanceRepository.Get(clientId, token);
    }
}