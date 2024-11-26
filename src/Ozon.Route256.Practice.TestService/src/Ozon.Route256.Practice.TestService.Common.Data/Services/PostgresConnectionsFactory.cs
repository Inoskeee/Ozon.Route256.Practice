using System.Data.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Ozon.Route256.TestService.Common.Data;

public class PostgresConnectionsFactory<TOptions> : IPostgresConnectionFactory<TOptions>, IDisposable, IAsyncDisposable
    where TOptions : DataConnectionOptions
{
    private readonly NpgsqlDataSource _dataSource;

    public PostgresConnectionsFactory(IOptionsSnapshot<TOptions> options)
    {
        _dataSource = NpgsqlDataSource.Create(options.Value.ConnectionString);
    }

    public DbConnection GetConnection() => _dataSource.CreateConnection();

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}
