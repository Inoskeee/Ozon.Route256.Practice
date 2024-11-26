using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ozon.Route256.TestService.Common.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDbFacade<TDataConnectionOptions>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TDataConnectionOptions : DataConnectionOptions
    {
        return services
            .Configure<TDataConnectionOptions>(configuration)
            .AddScoped<IPostgresConnectionFactory<TDataConnectionOptions>, PostgresConnectionsFactory<TDataConnectionOptions>>()
            .AddScoped<IDbFacade<TDataConnectionOptions>, DbFacade<TDataConnectionOptions>>();
    }
}
