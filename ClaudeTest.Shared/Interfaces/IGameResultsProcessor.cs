namespace ClaudeTest.Interfaces;

public interface IGameResultsProcessor
{
    Task ProcessAsync(Guid oid);
}
