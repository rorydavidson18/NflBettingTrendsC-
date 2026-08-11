using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClaudeTest.Pages.Teams;

[AllowAnonymous]
public class TeamQuery : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? TeamName { get; set; }

    [BindProperty(SupportsGet = true)]
    public long? TeamId { get; set; }

    public List<TeamEntity> Teams { get; set; } = new();

    private readonly ITeamsService teamsService;

    public TeamQuery(ITeamsService teamsService) => this.teamsService = teamsService;

    public async Task OnGetAsync()
    {
        if (TeamId.HasValue)
        {
            var team = await teamsService.GetTeamById(TeamId.Value);

            if (team != null)
            {
                Teams = new List<TeamEntity>()
                {
                    team
                };
            }

            return;
        }

        if (TeamName != null)
        {
            Teams = await teamsService.GetTeamsByName(TeamName);
            return;
        }

        Teams = await teamsService.GetAllTeams();
    }
}