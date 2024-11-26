using Ozon.Route256.Practice.ClientBalance.Bll.Models;

namespace Ozon.Route256.Practice.ClientBalance.Presentation.Mappers.Contracts;

public interface IClientRequestMapper
{
    ClientModel MapDtoToModel(CreateClientRequest createClientRequest);
}