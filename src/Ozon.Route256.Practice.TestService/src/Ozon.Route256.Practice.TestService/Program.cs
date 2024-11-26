using Ozon.Route256.TestService;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Host
    .CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(configure =>
    {
        configure.ConfigureAppConfiguration(builder =>
        {
            builder.Add(new TestServiceConfigurationSource());
        });
        configure.UseStartup<Startup>();
    })
    .UseDefaultServiceProvider((context, options) =>
    {
        var isValidationEnabled = !context.HostingEnvironment.IsProduction();

        options.ValidateScopes = isValidationEnabled;
        options.ValidateOnBuild = isValidationEnabled;
    })
    .Build()
    .Run();
