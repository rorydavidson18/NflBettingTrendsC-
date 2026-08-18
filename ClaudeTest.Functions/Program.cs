using Azure.Messaging.ServiceBus;
using ClaudeTest.Interfaces;
using ClaudeTest.Shared.Data;
using ClaudeTest.Shared.Interfaces;
using ClaudeTest.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<NflDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton(new ServiceBusClient(
            context.Configuration.GetConnectionString("ServiceBus")));

        services
            .AddScoped<IUserBetsService, UserBetsService>()
            .AddScoped<IServiceBusService, ServiceBusService>()
            .AddScoped<IGameResultsProcessor, GameResultsProcessor>();
    })
    .Build();

host.Run();
