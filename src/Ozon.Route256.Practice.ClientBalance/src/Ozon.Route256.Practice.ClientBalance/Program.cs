using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.ClientBalance;
using Ozon.Route256.Practice.ClientBalance.Dal.Migrations.Database;

await Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
        .UseStartup<Startup>()
        .ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5401, o => o.Protocols = HttpProtocols.Http1);
            options.Listen(IPAddress.Any, 5402, o => o.Protocols = HttpProtocols.Http2);
        }))
    .Build()
    .MigrateDatabase()
    .RunAsync();
