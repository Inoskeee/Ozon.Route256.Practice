using Ozon.Route256.Practice.ClientBalance.Bll.Models;
using Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Mappers;

public class ClientRequestMapper : IClientRequestMapper
{
    public ClientModel MapDtoToModel(CreateClientRequest createClientRequest)
    {
        return new ClientModel(
            createClientRequest.ClientId,
            createClientRequest.ClientName);
    }
}