using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

namespace Ozon.Route256.Practice.OrdersFacade;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        try
        {
            Log.Information("Application Starting.");
            
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The Application failed to start.");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder
                .UseStartup<Startup>()
                .ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 5101, o => o.Protocols = HttpProtocols.Http1);
                    options.Listen(IPAddress.Any, 5102, o => o.Protocols = HttpProtocols.Http2);
                }));
    }

}