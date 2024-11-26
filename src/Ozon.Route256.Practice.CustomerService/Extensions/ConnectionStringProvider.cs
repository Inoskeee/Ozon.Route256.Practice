namespace Ozon.Route256.CustomerService.Extensions;

public static class ConnectionStringProvider
{
    public static string? GetConnectionString()
    {
        return Environment.GetEnvironmentVariable("ROUTE256_CUSTOMER_SERVICE_DB_CONNECTION_STRING");
    }
}