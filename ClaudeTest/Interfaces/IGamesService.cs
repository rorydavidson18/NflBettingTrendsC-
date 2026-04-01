using ClaudeTest.Entities;
using ClaudeTest.Models;

namespace ClaudeTest.Interfaces;

public interface IGamesService
{
    public Task<List<GameEntity>> GetGames(long? gameId, string? team1, string? team2, bool? careAboutHomeAndAway,
        double? spread, bool spreadGreaterThanOrEqualTo, double? total, bool totalGreaterThanOrEqualTo);

    public TrendsModel CalculateTrends(List<GameEntity> games);

    public Task InsertGame(GameInsertModel model);
}