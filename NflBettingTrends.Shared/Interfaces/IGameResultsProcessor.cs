namespace NflBettingTrends.Interfaces;

public interface IGameResultsProcessor
{
    Task ProcessAsync(Guid oid);
}
