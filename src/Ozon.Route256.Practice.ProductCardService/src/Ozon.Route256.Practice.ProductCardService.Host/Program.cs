using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.ProductCardService.Host;

await Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
        .UseStartup<Startup>()
        .ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5801, o => o.Protocols = HttpProtocols.Http1);
            options.Listen(IPAddress.Any, 5802, o => o.Protocols = HttpProtocols.Http2);
        }))
    .Build()
    .RunAsync();