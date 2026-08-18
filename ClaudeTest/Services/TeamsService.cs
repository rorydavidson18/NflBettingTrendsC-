using ClaudeTest.Interfaces;
using ClaudeTest.Shared.Data;
using ClaudeTest.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Services;

public class TeamsService : ITeamsService
{
    private readonly NflDbContext dbContext;
    
    public TeamsService(NflDbContext dbContext) => this.dbContext = dbContext;

    public async Task<List<TeamEntity>> GetAllTeams()
    {
        return await dbContext.Teams
            .Include(e => e.Division)
            .ThenInclude(e => e.Conference)
            .OrderBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<TeamEntity?> GetTeamById(long id)
    {
        return await dbContext.Teams
            .Include(e => e.Division)
            .ThenInclude(e => e.Conference)
            .SingleAsync(e => e.Id == id);
    }

    public async Task<List<TeamEntity>> GetTeamsByName(string name)
    {
        return await dbContext.Teams
            .Where(e => e.Name.Contains(name))
            .Include(e => e.Division)
            .ThenInclude(e => e.Conference)
            .OrderBy(i => i.Id)
            .ToListAsync();
    }
}