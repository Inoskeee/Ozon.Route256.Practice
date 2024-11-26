using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Repositories;
using Ozon.Route256.Practice.ClientBalance.IntegrationTests.Helpers;

namespace Ozon.Route256.Practice.ClientBalance.IntegrationTests.Tests;

public class ClientRepositoryTests : BaseTestRepository, IClassFixture<PostgreSqlFixture>
{
    private readonly IClientRepository _clientRepository;
    private readonly CancellationToken _token = CancellationToken.None;

    public ClientRepositoryTests()
    {
        _clientRepository = new ClientRepository();
    }

    [Fact]
    public async Task Insert_ShouldInsertClientIntoDatabase()
    {
        // Arrange
        var client = new ClientEntity(200, "Test Client");

        // Act
        var result = await _clientRepository.Insert(client, _token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(client.ClientId, result);
        
        const string sqlQuery = @"
                select * from clients 
                where client_id = @ClientId";
        
        var param = new DynamicParameters();
        param.Add("ClientId", 200);

        var insertedClient = (await ExecuteQueryAsync<ClientEntity>(sqlQuery, param, _token)).FirstOrDefault();

        Assert.NotNull(insertedClient);
        Assert.Equal(client.ClientId, insertedClient.ClientId);
        Assert.Equal(client.ClientName, insertedClient.ClientName);
    }
}