using Ozon.Route256.Practice.ClientBalance.Bll.Extensions;
using Ozon.Route256.Practice.ClientBalance.Dal.Extensions;
using Ozon.Route256.Practice.ClientBalance.Presentation.Controllers.Grpc;
using Ozon.Route256.Practice.ClientBalance.Presentation.Extensions;

namespace Ozon.Route256.Practice.ClientBalance;

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

        services.ConfigureDalServices();
        services.ConfigureBllServices();
        services.ConfigurePresentationServices();
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
                endpoints.MapGrpcService<ClientBalanceGrpcService>();
                endpoints.MapGrpcReflectionService();
            });

    }
}