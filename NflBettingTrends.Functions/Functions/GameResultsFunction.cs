using NflBettingTrends.Interfaces;
using NflBettingTrends.Shared.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace NflBettingTrends.Functions.Functions;

public class GameResultsFunction
{
    private readonly ILogger<GameResultsFunction> logger;
    private readonly IGameResultsProcessor gameResultsProcessor;

    public GameResultsFunction(ILogger<GameResultsFunction> logger, IGameResultsProcessor gameResultsProcessor)
    {
        this.logger = logger;
        this.gameResultsProcessor = gameResultsProcessor;
    }

    [Function("ProcessGameResult")]
    public async Task Run([ServiceBusTrigger("game-results", Connection = "ServiceBus")] GameResultMessage message)
    {
        logger.LogInformation($"Processing game-results message for Oid: {message.Oid}");

        await gameResultsProcessor.ProcessAsync(message.Oid);
    }
}
