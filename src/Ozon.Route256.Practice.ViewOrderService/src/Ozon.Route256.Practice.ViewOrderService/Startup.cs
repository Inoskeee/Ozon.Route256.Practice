using Ozon.Route256.Practice.ViewOrderService.Infra.Kafka.Extensions;
using Ozon.Route256.Practice.ViewOrderService.Infra.OrdersService.Extensions;
using Ozon.Route256.Practice.ViewOrderService.Infra.Postgres.Extensions;

namespace Ozon.Route256.Practice.ViewOrderService;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddRouting();
        
        services.ConfigureOrdersServiceInfrastructure();
        services.ConfigurePostgresInfrastructure();
        services.ConfigureKafkaInfrastructure(_configuration);
    }

    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseRouting();
    }
}