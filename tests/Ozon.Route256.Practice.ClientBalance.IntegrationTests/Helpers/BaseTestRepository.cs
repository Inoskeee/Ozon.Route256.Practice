namespace Ozon.Route256.Practice.ClientBalance.IntegrationTests.Helpers;

public class BaseTestRepository
{
    private const string DbConnectionString = "ROUTE256_VIEW_ORDER_SERVICE_DB_CONNECTION_STRINGS";

    protected async Task<TModel[]> ExecuteQueryAsync<TModel>(
        string sql,
        object param,
        CancellationToken token)
    {
        var command = new CommandDefinition(sql, param, cancellationToken: token);

        await using var connection = GetConnection();
        var result = await connection.QueryAsync<TModel>(command);
        return result.ToArray();
    }
    
    protected async Task<TResult?> ExecuteNonQueryAsync<TResult>(
        string sql,
        object param,
        CancellationToken token)
    {
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: CommandTimeout.Medium,
            cancellationToken: token);

        await using var connection = GetConnection();

        return (TResult?)await connection.ExecuteScalarAsync(command);
    }

    private NpgsqlConnection GetConnection()
    {
        var connectionString = Environment.GetEnvironmentVariable(DbConnectionString);
        return new NpgsqlConnection(connectionString);
    }
    
    private static class CommandTimeout
    {
        public static int Medium => 5;
        
        public static int Long => 30;
    }
}