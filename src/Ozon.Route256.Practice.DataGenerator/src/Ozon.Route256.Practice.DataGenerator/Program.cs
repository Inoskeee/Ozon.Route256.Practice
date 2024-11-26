using Ozon.Route256.DataGenerator.Bll.DI;
using Ozon.Route256.DataGenerator.Infra.DI;

var builder = WebApplication.CreateBuilder(args);


var config = builder.Configuration
    .AddEnvironmentVariables("ROUTE256_")
    .Build();

builder.Services
    .AddBll()
    .AddInfra(config)
    .AddHealthChecks();

var app = builder.Build();

app.UseRouting();
app.MapHealthChecks("/health");

app.Run();
