using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ClaudeTest.Interfaces;
using ClaudeTest.Shared.Interfaces;

namespace ClaudeTest.Shared.Services;

public class ServiceBusService : IServiceBusService
{
    private readonly IUserBetsService userBetsService;
    private readonly ServiceBusClient client;
    private const string QueueName = "game-results";

    public ServiceBusService(ServiceBusClient client, IUserBetsService userBetsService)
    {
        this.client = client;
        this.userBetsService = userBetsService;
    }

    public async Task PublishGameResultAsync()
    {
        var users  = await userBetsService.GetDistinctUserIds();
        var sender = client.CreateSender(QueueName);

        var messages = users.Select(e => new ServiceBusMessage(JsonSerializer.Serialize(new
        {
            Oid = e
        })));

        await sender.SendMessagesAsync(messages);
    }
}