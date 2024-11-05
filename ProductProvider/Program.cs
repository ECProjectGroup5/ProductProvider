using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
     .ConfigureAppConfiguration((context, config) =>
     {
         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
         config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
         config.AddEnvironmentVariables();
     })

    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("Database");
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
        services.AddScoped<ProductService>();
        services.AddScoped<Repo>();
        services.AddScoped<ProductFactory>();
        services.AddScoped<IRepo, Repo>();


    })
    .Build();

host.Run();