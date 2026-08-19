using Azure.Messaging.ServiceBus;
using NflBettingTrends.Interfaces;
using NflBettingTrends.Shared.Data;
using NflBettingTrends.Shared.Interfaces;
using NflBettingTrends.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<NflDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.AddSingleton(new ServiceBusClient(
            context.Configuration.GetConnectionString("ServiceBus")));

        services
            .AddScoped<IUserBetsService, UserBetsService>()
            .AddScoped<IServiceBusService, ServiceBusService>()
            .AddScoped<IGameResultsProcessor, GameResultsProcessor>();
    })
    .Build();

host.Run();
