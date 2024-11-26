using Ozon.Route256.Practice.ClientOrders.Bll.Extensions;
using Ozon.Route256.Practice.ClientOrders.Infra.CustomerService.Extensions;
using Ozon.Route256.Practice.ClientOrders.Infra.Kafka.Extensions;
using Ozon.Route256.Practice.ClientOrders.Infra.OrdersService.Extensions;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Extensions;
using Ozon.Route256.Practice.ClientOrders.Presentation.Controllers.Grpc;
using Ozon.Route256.Practice.ClientOrders.Presentation.Extensions;

namespace Ozon.Route256.Practice.ClientOrders;

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

        services.ConfigurePostgresInfrastructure();
        services.ConfigureCustomerServiceInfrastructure();
        services.ConfigureOrdersServiceInfrastructure();
        services.ConfigureKafkaInfrastructure(_configuration);
        
        services.ConfigureBllServices(_configuration);
        
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
                endpoints.MapGrpcService<ClientOrdersGrpcService>();
                endpoints.MapGrpcReflectionService();
            });

    }
}