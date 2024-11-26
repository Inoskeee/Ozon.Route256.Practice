using FluentValidation.AspNetCore;
using Ozon.Route256.Practice.OrdersFacade.Bll;
using Ozon.Route256.Practice.OrdersFacade.Bll.Extensions;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Extensions;
using Ozon.Route256.Practice.OrdersFacade.Bll.Grpc.Interceptors;
using Ozon.Route256.Practice.OrdersFacade.Bll.Http.Extensions;
using Ozon.Route256.Practice.OrdersFacade.Presentation;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Extensions;
using Ozon.Route256.Practice.OrdersFacade.Presentation.Infrastructure;
using Serilog;

namespace Ozon.Route256.Practice.OrdersFacade;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSerilog();
        
        services.AddFluentValidation();
        
        services.ConfigureBllServices();
        services.ConfigureGrpcServices();
        services.ConfigureHttpServices(_configuration);
        services.ConfigurePresentation();
    }

    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseSerilogRequestLogging();
        
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI();
        applicationBuilder.UseRouting();

        applicationBuilder.UseMiddleware<LoggerMiddleware>();
        
        applicationBuilder.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<OrderFacadeGrpcService>();
                endpoints.MapGrpcReflectionService();
                endpoints.MapPrometheusScrapingEndpoint();
            });
    }
}