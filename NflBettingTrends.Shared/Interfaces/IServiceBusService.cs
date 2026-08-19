namespace NflBettingTrends.Shared.Interfaces;

public interface IServiceBusService
{
    public Task PublishGameResultAsync();
}