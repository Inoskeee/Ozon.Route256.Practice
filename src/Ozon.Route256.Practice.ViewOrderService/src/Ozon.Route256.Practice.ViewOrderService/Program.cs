using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.ViewOrderService;

await Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
        .UseStartup<Startup>()
        .ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5701, o => o.Protocols = HttpProtocols.Http1);
        }))
    .Build()
    .RunAsync();