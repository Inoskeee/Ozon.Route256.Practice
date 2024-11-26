using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.OrdersReport;

await Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
        .UseStartup<Startup>()
        .ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5501, o => o.Protocols = HttpProtocols.Http1);
        }))
    .Build()
    .RunAsync();