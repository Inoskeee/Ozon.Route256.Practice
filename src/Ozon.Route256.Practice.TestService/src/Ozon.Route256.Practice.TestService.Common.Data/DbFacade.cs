using Dapper;

namespace Ozon.Route256.TestService.Common.Data;

public class DbFacade<TOptions> : IDbFacade<TOptions>
    where TOptions : DataConnectionOptions
{
    private readonly IPostgresConnectionFactory<TOptions> _connectionFactory;

    public DbFacade(IPostgresConnectionFactory<TOptions> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition command)
    {
        await using var connection = _connectionFactory.GetConnection();

        return await connection.QueryAsync<T>(command);
    }
}
