using ClaudeTest.Data;
using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using ClaudeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Services;

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
            Payout = 0 - model.BetSize
        };

        dbContext.UserBets.Add(entity);

        await dbContext.SaveChangesAsync();
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
