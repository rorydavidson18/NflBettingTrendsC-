using ClaudeTest.Data;
using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using ClaudeTest.Mappers;
using ClaudeTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Services;

public class GamesService : IGamesService
{
    private readonly NflDbContext dbContext;

    public GamesService(NflDbContext dbContext) => this.dbContext = dbContext;

    public async Task<List<GameEntity>> GetGames()
    {
        return await dbContext.Games
            .Include(e => e.HomeTeam)
            .Include(e => e.AwayTeam)
            .ToListAsync();
    }

    public async Task<List<GameEntity>> GetGames(long? gameId, string? team1, string? team2, bool? careAboutHomeAndAway,
        double? spread, bool spreadGreaterThanOrEqualTo, double? total, bool totalGreaterThanOrEqualTo,
        DateTime startDate, DateTime endDate)
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

        games = games.Where(e => e.Date >= startDate && e.Date <= endDate);
        
        //TODO make a playoff game search

        return await games
            .Include(e => e.HomeTeam)
            .Include(e => e.AwayTeam)
            .ToListAsync();
    }

    public TrendsModel CalculateTrends(List<GameEntity> games)
    {
        var totalGames = games.Count;

        if (totalGames == 0)
        {
            return new TrendsModel()
            {
                SpreadPercentage = 0,
                TotalPercentage = 0
            };
        }

        double gamesHomeTeamCovered = games.Count(e => e.SpreadResult > 0);

        double gamesOverTotal = games.Count(e => e.TotalResult > 0);

        return new TrendsModel()
        {
            SpreadPercentage = (gamesHomeTeamCovered / totalGames) * 100,
            TotalPercentage = (gamesOverTotal / totalGames) * 100
        };
    }

    // Ordered biggest home underdog -> pick'em -> biggest home favorite.
    // Spread is negative when the home team is favored (see GameEntity.SpreadResult).
    private static readonly (double Low, double High, string Label)[] AtsBinDefinitions =
    [
        (21.5, double.MaxValue, "+21.5 or more"),
        (17.5, 21, "+17.5 to +21"),
        (14, 17, "+14 to +17"),
        (10, 13.5, "+10 to +13.5"),
        (7.5, 9.5, "+7.5 to +9.5"),
        (5, 7, "+5 to +7"),
        (3, 4.5, "+3 to +4.5"),
        (1, 2.5, "+1 to +2.5"),
        (-0.5, 0.5, "Pick'em"),
        (-2.5, -1, "-1 to -2.5"),
        (-4.5, -3, "-3 to -4.5"),
        (-7, -5, "-5 to -7"),
        (-9.5, -7.5, "-7.5 to -9.5"),
        (-13.5, -10, "-10 to -13.5"),
        (-17, -14, "-14 to -17"),
        (-21, -17.5, "-17.5 to -21"),
        (double.MinValue, -21.5, "-21.5 or more"),
    ];

    public List<AtsBinModel> CalculateAtsBins(List<GameEntity> games)
    {
        // Pushes (SpreadResult == 0) are excluded entirely, not just counted as a non-cover.
        var decidedGames = games.Where(e => e.SpreadResult != 0).ToList();

        return AtsBinDefinitions.Select(bin =>
        {
            var gamesInBin = decidedGames.Where(e => e.Spread >= bin.Low && e.Spread <= bin.High).ToList();

            return new AtsBinModel
            {
                Label = bin.Label,
                GameCount = gamesInBin.Count,
                CoverPercentage = gamesInBin.Count == 0
                    ? 0
                    : (double)gamesInBin.Count(e => e.SpreadResult > 0) / gamesInBin.Count * 100
            };
        }).ToList();
    }

    public async Task InsertGame(GameInsertModel model)
    {
        var homeTeamId = await dbContext.Teams.SingleAsync(e => e.Abbreviation == model.HomeTeamAbv);
        var awayTeamId = await dbContext.Teams.SingleAsync(e => e.Abbreviation == model.AwayTeamAbv);
        
        var entity = ModelMapper.ToEntity(model, homeTeamId.Id, awayTeamId.Id);
        dbContext.Games.Add(entity);

        await dbContext.SaveChangesAsync();
    }
}