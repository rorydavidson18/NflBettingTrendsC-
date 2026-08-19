using NflBettingTrends.Interfaces;
using NflBettingTrends.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NflBettingTrends.Shared.Services;

public class GameResultsProcessor : IGameResultsProcessor
{
    private readonly NflDbContext dbContext;

    public GameResultsProcessor(NflDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task ProcessAsync(Guid oid)
    {
        var userBets = await dbContext.UserBets
            .Where(e => e.Oid == oid)
            .Include(e => e.Game)
            .Include(e => e.Team)
            .ToListAsync();

        foreach (var userBet in userBets)
        {
            var game = userBet.Game;

            if (game.HomeScore == 0 && game.AwayScore == 0)
            {
                continue;
            }

            var spreadResult = game.SpreadResult;

            if (spreadResult == 0)
            {
                userBet.Payout = 0;
            }
            else if (spreadResult > 0)
            {
                if (userBet.TeamId == game.HomeTeamId)
                {
                    userBet.Payout = CalculatePayout(userBet.BetSize, userBet.Odds);
                }
            }
            else
            {
                if (userBet.TeamId == game.AwayTeamId)
                {
                    userBet.Payout = CalculatePayout(userBet.BetSize, userBet.Odds);
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private static decimal CalculatePayout(decimal betSize, double odds)
    {
        var oddsDecimal = (decimal)odds;

        return odds < 0
            ? betSize + betSize * (100m / Math.Abs(oddsDecimal))
            : betSize + betSize * (oddsDecimal / 100m);
    }
}