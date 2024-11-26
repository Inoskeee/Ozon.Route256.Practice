using Ozon.Route256.Practice.ClientBalance.Bll.Contracts.Mappers;
using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Bll.Mappers;

internal sealed class ClientMapper : IClientMapper
{
    public ClientEntity MapModelToEntity(ClientModel clientModel)
    {
        return new ClientEntity(
            clientModel.ClientId,
            clientModel.ClientName);
    }
}