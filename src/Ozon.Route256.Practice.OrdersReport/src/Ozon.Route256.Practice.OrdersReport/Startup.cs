using Ozon.Route256.Practice.OrdersReport.Bll.Extensions;
using Ozon.Route256.Practice.OrdersReport.Presentation.Extensions;
using Ozon.Route256.Practice.OrdersReport.Presentation.Infrastructure;

namespace Ozon.Route256.Practice.OrdersReport;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureBllServices(_configuration);
        services.ConfigurePresentation();
    }

    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI();
        applicationBuilder.UseRouting();

        applicationBuilder.UseMiddleware<LoggerMiddleware>();

        applicationBuilder.UseEndpoints(
            endpoints => { endpoints.MapControllers(); });
    }
}