using System.Data;
using System.Data.Common;
using Npgsql;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Connection;

namespace Route256.SharderRepository.Infrastructure.Connection;

public class ShardNpgsqlConnection: DbConnection
{
    private readonly NpgsqlConnection _npgsqlConnection;

    public ShardNpgsqlConnection(NpgsqlConnection npgsqlConnection, int bucketId)
    {
        _npgsqlConnection = npgsqlConnection;
        BucketId          = bucketId;
    }

    public int BucketId { get; }

    protected override DbCommand CreateDbCommand()
    {
        var command = _npgsqlConnection.CreateCommand();
        return new ShardNpgsqlCommand(command, BucketId);
    }
    
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => 
        _npgsqlConnection.BeginTransaction(isolationLevel);

    public override void ChangeDatabase(string databaseName) => 
        _npgsqlConnection.ChangeDatabase(databaseName);

    public override void Close() => _npgsqlConnection.Close();

    public override void Open() => _npgsqlConnection.Open();

    public override string ConnectionString
    {
        get => _npgsqlConnection.ConnectionString ?? string.Empty;
        set => _npgsqlConnection.ConnectionString = value;
    }

    public override string Database => _npgsqlConnection.Database;
    public override ConnectionState State => _npgsqlConnection.State;
    public override string DataSource => _npgsqlConnection.DataSource;
    public override string ServerVersion => _npgsqlConnection.ServerVersion;

    public override ValueTask DisposeAsync()
    {
        return _npgsqlConnection.DisposeAsync();
    }
}