using FluentValidation;

using Ozon.Route256.CustomerService.Extensions;
using Ozon.Route256.CustomerService.Presentation.Controllers.Grpc;
using Ozon.Route256.CustomerService.Presentation.Interceptors;

namespace Ozon.Route256.CustomerService;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var currentAssembly = typeof(Startup).Assembly;
        services
            .AddMediatR(c => c.RegisterServicesFromAssembly(currentAssembly));

        services
            .AddDatabase()
            .AddRepositories();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        services.AddControllers();
        services.AddGrpc(
                options =>
                {
                    options.EnableDetailedErrors = true;
                    options.Interceptors.Add<GrpcExceptionInterceptor>();
                    options.Interceptors.Add<GrpcRequestValidationInterceptor>();
                })
            .AddJsonTranscoding();
        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddGrpcSwagger();
        services.AddSwaggerGen();
        services.AddGrpcReflection();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<CustomerController>();
                endpoints.MapGrpcReflectionService();
            });
    }
}