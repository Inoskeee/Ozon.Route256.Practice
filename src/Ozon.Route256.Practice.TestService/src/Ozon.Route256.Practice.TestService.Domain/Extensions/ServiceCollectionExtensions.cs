using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.TestService.Domain.Metrics;

namespace Ozon.Route256.TestService.Domain.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services
            .AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IDomainMarker>())
            .AddSingleton<MismatchMetricManager>()
            .AddSingleton<IMismatchMetricReporter>(provider => provider.GetRequiredService<MismatchMetricManager>())
            .AddSingleton<IMismatchStatisticsReader>(provider => provider.GetRequiredService<MismatchMetricManager>());
}
