using ClaudeTest.Models;
using ClaudeTest.Shared.Entities;

namespace ClaudeTest.Interfaces;

public interface IGamesService
{
    public Task<List<GameEntity>> GetGames();

    public Task<List<GameEntity>> GetGames(long? gameId, string? team1, string? team2, bool? careAboutHomeAndAway,
        double? spread, bool spreadGreaterThanOrEqualTo, double? total, bool totalGreaterThanOrEqualTo, 
        DateTime startDate, DateTime endDate);

    public TrendsModel CalculateTrends(List<GameEntity> games);

    public List<AtsBinModel> CalculateAtsBins(List<GameEntity> games);

    public Task InsertGame(GameInsertModel model);
}