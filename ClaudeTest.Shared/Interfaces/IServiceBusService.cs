namespace ClaudeTest.Shared.Interfaces;

public interface IServiceBusService
{
    public Task PublishGameResultAsync();
}