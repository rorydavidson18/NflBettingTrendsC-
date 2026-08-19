using NflBettingTrends.Models;
using NflBettingTrends.Shared.Entities;

namespace NflBettingTrends.Mappers;

public static class ModelMapper
{
    public static GameEntity ToEntity(GameInsertModel model, long homeTeamId, long awayTeamId)
    {
        return new GameEntity()
        {
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            Date = model.Date,
            Spread = model.Spread,
            HomeScore = model.HomeScore,
            AwayScore = model.AwayScore,
            Total = model.Total,
            PlayoffGame = model.PlayoffGame
        };
    }
}