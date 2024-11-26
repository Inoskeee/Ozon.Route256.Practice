using System.Data.Common;

namespace Ozon.Route256.TestService.Common.Data;

public interface IPostgresConnectionFactory<TOptions>
    where TOptions : DataConnectionOptions
{
    DbConnection GetConnection();
}
