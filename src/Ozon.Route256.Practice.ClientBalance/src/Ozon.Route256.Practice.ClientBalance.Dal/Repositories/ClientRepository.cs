using Dapper;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts;
using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Repositories;

public sealed class ClientRepository : BaseRepository, IClientRepository
{
    public async Task<long?> Insert(ClientEntity client, CancellationToken token)
    {
        const string sqlQuery = @"
                insert into clients(client_id, client_name)
                values (@ClientId, @ClientName)
                returning client_id";

        var param = new DynamicParameters();
        param.Add("ClientId", client.ClientId);
        param.Add("ClientName", client.ClientName);
        
        return await ExecuteNonQueryAsync<long?>(sqlQuery, param, token);
    }
}