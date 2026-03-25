using ClaudeTest.Entities;

namespace ClaudeTest.Interfaces;

public interface ITeamsService
{
    public Task<List<TeamEntity>> GetAllTeams();
    public Task<TeamEntity?> GetTeamById(long id);
    public Task<List<TeamEntity>> GetTeamsByName(string name);
}