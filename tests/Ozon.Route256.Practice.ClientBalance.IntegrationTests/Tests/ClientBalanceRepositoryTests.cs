using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Repositories;
using Ozon.Route256.Practice.ClientBalance.IntegrationTests.Helpers;

namespace Ozon.Route256.Practice.ClientBalance.IntegrationTests.Tests;

public class ClientBalanceRepositoryTests : BaseTestRepository, IClassFixture<PostgreSqlFixture>
{
    private readonly ClientBalanceRepository _clientBalanceRepository;
    private readonly ClientRepository _clientRepository;
    private readonly CancellationToken _token = CancellationToken.None;

    public ClientBalanceRepositoryTests()
    {
        _clientBalanceRepository = new ClientBalanceRepository();
        _clientRepository = new ClientRepository();
    }

    [Fact]
    public async Task Get_ShouldReturnEmptyWhenNoBalance()
    {
        // Arrange
        var clientId = 100;

        // Act
        var balances = await _clientBalanceRepository.Get(clientId, _token);

        // Assert
        Assert.NotNull(balances);
        Assert.Empty(balances);
    }

    [Fact]
    public async Task UpdateOrInsert_TopUp_ShouldIncreaseBalance()
    {
        // Arrange
        var client = new ClientEntity(101, "Test Client");
        await _clientRepository.Insert(client, _token);

        var balance = new ClientBalanceEntity(
            client.ClientId,
            "RUB",
            100,
            100000000);

        // Act
        var result = await _clientBalanceRepository.UpdateOrInsert(balance, true, _token);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(client.ClientId, result.Result);
        
        var balances = await _clientBalanceRepository.Get(client.ClientId, _token);
        Assert.Single(balances);

        var retrievedBalance = balances.First();
        Assert.Equal(balance.ClientId, retrievedBalance.ClientId);
        Assert.Equal(balance.CurrencyCode, retrievedBalance.CurrencyCode);
        Assert.Equal(balance.Units, retrievedBalance.Units);
        Assert.Equal(balance.Nanos, retrievedBalance.Nanos);
    }
    
}