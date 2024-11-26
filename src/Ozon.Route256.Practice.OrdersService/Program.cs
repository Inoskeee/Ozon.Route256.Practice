using FluentValidation;
using Ozon.Route256.OrderService.Extensions;
using Ozon.Route256.OrderService.Grpc;
using Ozon.Route256.OrderService.Grpc.Interceptors;
using Ozon.Route256.OrderService.Migrations.Database;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration
    .AddEnvironmentVariables("ROUTE256_")
    .Build();

builder.Services
    .AddDatabase(config)
    .AddRepositories()
    .AddServices()
    .AddKafka(config);

builder.Services.AddControllers();
builder.Services.AddGrpc(
        options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<GrpcExceptionInterceptor>();
            options.Interceptors.Add<GrpcValidationInterceptor>();
        })
    .AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpcReflection();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();

app.MigrateDatabase();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseEndpoints(
    endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapGrpcService<OrderGrpcService>();
        endpoints.MapGrpcReflectionService();
    });

app.Run();
