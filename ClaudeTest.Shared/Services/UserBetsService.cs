using ClaudeTest.Interfaces;
using ClaudeTest.Shared.Data;
using ClaudeTest.Shared.Entities;
using ClaudeTest.Shared.Interfaces;
using ClaudeTest.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Shared.Services;

public class UserBetsService : IUserBetsService
{
    private readonly NflDbContext dbContext;
    
    public UserBetsService(NflDbContext dbContext) => this.dbContext = dbContext;

    public async Task InsertUserBet(UserBetInsertModel model, Guid oid)
    {
        var entity = new UserBetEntity
        {
            Oid = oid,
            Date = DateTime.UtcNow,
            BetSize = model.BetSize,
            Odds = model.Odds,
            GameId = model.GameId,
            Payout = 0 - model.BetSize,
            TeamId = model.TeamId
        };

        dbContext.UserBets.Add(entity);

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Guid>> GetDistinctUserIds()
    {
        return await dbContext.UserBets
            .Select(e => e.Oid)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<UserBetEntity>> GetUserBets(Guid oid)
    {
        return await dbContext.UserBets
            .Where(e => e.Oid == oid)
            .Include(e => e.Game).ThenInclude(g => g.HomeTeam)
            .Include(e => e.Game).ThenInclude(g => g.AwayTeam)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }
}
