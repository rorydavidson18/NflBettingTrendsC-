using ClaudeTest.Interfaces;
using ClaudeTest.Shared.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ClaudeTest.Functions.Functions;

public class NightlyFunction
{
    private readonly ILogger<NightlyFunction> logger;
    private readonly IServiceBusService serviceBusService;
    
    public NightlyFunction(ILogger<NightlyFunction> logger, IServiceBusService serviceBusService)
    {
        this.logger = logger;
        this.serviceBusService = serviceBusService;
    }
    
    [Function("NightlyPublishUserOids")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timer)
    {
        logger.LogInformation($"NightlyPublishUserOids started at: {DateTime.UtcNow}");

        await serviceBusService.PublishGameResultAsync();

        logger.LogInformation($"Successfully published messages to Service Bus.");
    }
}