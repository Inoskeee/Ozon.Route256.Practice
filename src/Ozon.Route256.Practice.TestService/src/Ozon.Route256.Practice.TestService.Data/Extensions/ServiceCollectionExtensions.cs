using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ozon.Route256.TestService.Data;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMismatchDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<MismatchOptions>(configuration)
            .AddSingleton<IMismatchRepository, MismatchRepository>()
            .AddSingleton<MismatchFeature>()
            .AddSingleton<IMismatchFeature>(provider => provider.GetRequiredService<MismatchFeature>())
            .AddSingleton<IMismatchFeatureToggler>(provider => provider.GetRequiredService<MismatchFeature>());
    }
}
