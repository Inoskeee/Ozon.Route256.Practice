using Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.Enums;
using Ozon.Route256.Practice.ClientBalance.Dal.Entities.OperationEntities;
using Ozon.Route256.Practice.ClientBalance.Dal.Repositories;
using Ozon.Route256.Practice.ClientBalance.IntegrationTests.Helpers;

namespace Ozon.Route256.Practice.ClientBalance.IntegrationTests.Tests;

public class WithdrawOperationRepositoryTests: BaseTestRepository, IClassFixture<PostgreSqlFixture>
{
    private readonly ClientRepository _clientRepository;
    private readonly IOperationRepository<WithdrawOperationEntity> _withdrawOperationRepository;

    public WithdrawOperationRepositoryTests()
    {
        _withdrawOperationRepository = new WithdrawOperationRepository();
        _clientRepository = new ClientRepository();
    }

    [Fact]
    public async Task Insert_ShouldAddOperationSuccessfully()
    {
        // Arrange
        var clientId = 400;
        var client = new ClientEntity(clientId, "Test Client");
        await _clientRepository.Insert(client, CancellationToken.None);

        var operationGuid = Guid.NewGuid();
        var operation = new WithdrawOperationEntity()
        {
            Guid = operationGuid,
            ClientId = clientId,
            CurrencyCode = "RUB",
            Units = 100,
            Nanos = 0,
            OperationTypeEntity = OperationTypesEntity.TopUp,
            OperationStatus = OperationStatusesEntity.Pending,
            OperationTime = DateTimeOffset.UtcNow
        };

        // Act
        var result = await _withdrawOperationRepository.Insert(operation, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(operationGuid.ToString(), result.Result);

        // Проверяем, что операция сохранена
        var insertedOperation = await _withdrawOperationRepository.TryGet(operationGuid, CancellationToken.None);
        Assert.NotNull(insertedOperation);
        Assert.Equal(operation.Guid, insertedOperation.Guid);
        Assert.Equal(operation.ClientId, insertedOperation.ClientId);
        Assert.Equal(operation.CurrencyCode, insertedOperation.CurrencyCode);
        Assert.Equal(operation.Units, insertedOperation.Units);
        Assert.Equal(operation.Nanos, insertedOperation.Nanos);
        Assert.Equal(operation.OperationTypeEntity, insertedOperation.OperationTypeEntity);
        Assert.Equal(operation.OperationStatus, insertedOperation.OperationStatus);
        Assert.Equal(operation.OperationTime.Date, insertedOperation.OperationTime.Date);
    }

    [Fact]
    public async Task Update_ShouldUpdateOperationSuccessfully()
    {
        // Arrange
        long clientId = 401;
        var client = new ClientEntity(clientId, "Test Client");
        await _clientRepository.Insert(client, CancellationToken.None);

        var operationGuid = Guid.NewGuid();
        var operation = new WithdrawOperationEntity
        {
            Guid = operationGuid,
            ClientId = clientId,
            CurrencyCode = "RUB",
            Units = 100,
            Nanos = 0,
            OperationTypeEntity = OperationTypesEntity.TopUp,
            OperationStatus = OperationStatusesEntity.Pending,
            OperationTime = DateTimeOffset.UtcNow
        };

        await _withdrawOperationRepository.Insert(operation, CancellationToken.None);

        // Act
        var updatedEntity = new WithdrawOperationEntity
        {
            Guid = operationGuid,
            ClientId = clientId,
            CurrencyCode = "RUB",
            Units = 100,
            Nanos = 0,
            OperationTypeEntity = OperationTypesEntity.TopUp,
            OperationStatus = OperationStatusesEntity.Completed,
            OperationTime = DateTimeOffset.UtcNow
        };

        var result = await _withdrawOperationRepository.Update(updatedEntity, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(operationGuid.ToString(), result.Result);
        
        var updatedOperation = await _withdrawOperationRepository.TryGet(operationGuid, CancellationToken.None);
        Assert.NotNull(updatedOperation);
        Assert.Equal(OperationStatusesEntity.Completed, updatedOperation.OperationStatus);
        Assert.Equal(operation.OperationTime.Date, updatedOperation.OperationTime.Date);
    }

    [Fact]
    public async Task TryGet_ShouldRetrieveOperationSuccessfully()
    {
        // Arrange
        long clientId = 402;
        var client = new ClientEntity(clientId, "Test Client");
        await _clientRepository.Insert(client, CancellationToken.None);

        var operationGuid = Guid.NewGuid();
        var operation = new WithdrawOperationEntity
        {
            Guid = operationGuid,
            ClientId = clientId,
            CurrencyCode = "RUB",
            Units = 100,
            Nanos = 0,
            OperationTypeEntity = OperationTypesEntity.TopUp,
            OperationStatus = OperationStatusesEntity.Pending,
            OperationTime = DateTimeOffset.UtcNow
        };

        await _withdrawOperationRepository.Insert(operation, CancellationToken.None);

        // Act
        var resultOperation = await _withdrawOperationRepository.TryGet(operationGuid, CancellationToken.None);

        // Assert
        Assert.NotNull(resultOperation);
        Assert.Equal(operation.Guid, resultOperation.Guid);
        Assert.Equal(operation.ClientId, resultOperation.ClientId);
        Assert.Equal(operation.CurrencyCode, resultOperation.CurrencyCode);
        Assert.Equal(operation.Units, resultOperation.Units);
        Assert.Equal(operation.Nanos, resultOperation.Nanos);
        Assert.Equal(operation.OperationTypeEntity, resultOperation.OperationTypeEntity);
        Assert.Equal(operation.OperationStatus, resultOperation.OperationStatus);
        Assert.Equal(operation.OperationTime.Date, resultOperation.OperationTime.Date);
    }
}