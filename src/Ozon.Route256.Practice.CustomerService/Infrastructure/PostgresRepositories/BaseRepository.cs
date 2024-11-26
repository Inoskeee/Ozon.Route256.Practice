using Dapper;

using Npgsql;

using Ozon.Route256.CustomerService.Extensions;

namespace Ozon.Route256.CustomerService.Infrastructure.PostgresRepositories;

public class BaseRepository
{
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

    protected async Task ExecuteNonQueryAsync(
        string sql,
        object param,
        CancellationToken token)
    {
        var command = new CommandDefinition(
            sql, param,
            commandTimeout: CommandTimeout.Medium,
            cancellationToken: token);

        await using var connection = GetConnection();

        await connection.ExecuteAsync(command);
    }

    private NpgsqlConnection GetConnection()
    {
        var connectionString = ConnectionStringProvider.GetConnectionString()!;
        return new NpgsqlConnection(connectionString);
    }

    private static class CommandTimeout
    {
        public static int Medium => 5;

        public static int Long => 30;
    }
}