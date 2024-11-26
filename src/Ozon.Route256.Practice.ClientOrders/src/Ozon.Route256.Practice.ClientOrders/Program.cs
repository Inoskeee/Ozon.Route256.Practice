using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.ClientOrders;
using Ozon.Route256.Practice.ClientOrders.Infra.Postgres.Migrations.Database;

await Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
        .UseStartup<Startup>()
        .ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5601, o => o.Protocols = HttpProtocols.Http1);
            options.Listen(IPAddress.Any, 5602, o => o.Protocols = HttpProtocols.Http2);
        }))
    .Build()
    .MigrateDatabase()
    .RunAsync();