using Ozon.Route256.Practice.ProductCardService.Application.Extensions;
using Ozon.Route256.Practice.ProductCardService.Infrastructure.Extensions;
using Ozon.Route256.Practice.ProductCardService.Presentation.Controllers.Grpc;
using Ozon.Route256.Practice.ProductCardService.Presentation.Extensions;

namespace Ozon.Route256.Practice.ProductCardService.Host;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddRouting();

        services.ConfigureApplicationLayer();
        services.ConfigureInfrastructureLayer();
        services.ConfigurePresentationLayer();
    }

    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI();
        applicationBuilder.UseRouting();
        
        applicationBuilder.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ProductCardGrpcService>();
                endpoints.MapGrpcReflectionService();
            });

    }
}