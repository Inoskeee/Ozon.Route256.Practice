using Ozon.Route256.Practice.ClientBalance.Dal.Entities;

namespace Ozon.Route256.Practice.ClientBalance.Dal.Contracts.Repositories;

public interface IOperationRepository<TEntity>
{
    Task<TEntity?> TryGet(
        Guid guid,
        CancellationToken token);
    
    Task<ResultEntity<string?>> Insert(
        TEntity operation,
        CancellationToken token);
    
    Task<ResultEntity<string?>> Update(
        TEntity operation,
        CancellationToken token);
}