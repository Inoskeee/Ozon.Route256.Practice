using Ozon.Route256.CustomerService;
using Ozon.Route256.CustomerService.Infrastructure.Migrations.Database;

Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder
        .UseStartup<Startup>())
    .Build()
    .MigrateDatabase()
    .Run();