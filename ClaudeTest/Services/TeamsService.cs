using ClaudeTest.Data;
using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaudeTest.Services;

public class TeamsService : ITeamsService
{
    private readonly NflDbContext dbContext;
    
    public TeamsService(NflDbContext dbContext) => this.dbContext = dbContext;

    public async Task<List<TeamEntity>> GetAllTeams()
    {
        return await dbContext.Teams
            .OrderBy(i => i.Id)
            .ToListAsync();
    }

    public async Task<TeamEntity?> GetTeamById(long id)
    {
        return await dbContext.Teams.FindAsync(id);
    }

    public async Task<List<TeamEntity>> GetTeamsByName(string name)
    {
        return await dbContext.Teams
            .Where(e => e.Name.Contains(name))
            .OrderBy(i => i.Id)
            .ToListAsync();
    }
}