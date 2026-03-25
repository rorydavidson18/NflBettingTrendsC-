using ClaudeTest.Data;
using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Services;

public class GamesService : IGamesService
{
    private readonly NflDbContext dbContext;

    public GamesService(NflDbContext dbContext) => this.dbContext = dbContext;

    public async Task<List<GameEntity>> GetGames(long? gameId, string? team1, string? team2, bool? careAboutHomeAndAway,
        double? spread, bool spreadGreaterThanOrEqualTo, double? total, bool totalGreaterThanOrEqualTo)
    {
        var games = dbContext.Games.AsQueryable();

        if (gameId != null)
        {
            return await dbContext.Games.Where(e => e.Id == gameId).ToListAsync();
        }

        if (careAboutHomeAndAway.HasValue && careAboutHomeAndAway.Value)
        {
            if (team1 != null)
            {
                games = games.Where(e => e.HomeTeam.Abbreviation == team1);
            }

            if (team2 != null)
            {
                games = games.Where(e => e.AwayTeam.Abbreviation == team2);
            }
        }
        else
        {
            if (team1 != null)
            {
                games = games.Where(e => e.HomeTeam.Abbreviation == team1 || e.AwayTeam.Abbreviation == team1);
            }

            if (team2 != null)
            {
                games = games.Where(e => e.AwayTeam.Abbreviation == team2 || e.HomeTeam.Abbreviation == team2);
            }
        }

        if (spread != null)
        {
            if (spreadGreaterThanOrEqualTo)
            {
                games = games.Where(e => e.Spread >= spread);
            }
            else
            {
                games = games.Where(e => e.Spread <= spread);
            }
        }

        if (total != null)
        {
            if (totalGreaterThanOrEqualTo)
            {
                games = games.Where(e => e.Total >= total);
            }
            else
            {
                games = games.Where(e => e.Total <= total);
            }
        }
        
        //TODO make a playoff game search

        return await games
            .Include(e => e.HomeTeam)
            .Include(e => e.AwayTeam)
            .ToListAsync();
    }
}