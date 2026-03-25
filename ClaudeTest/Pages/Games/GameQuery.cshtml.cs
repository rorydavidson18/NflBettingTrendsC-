using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClaudeTest.Pages.Games;

public class GameQuery : PageModel
{
    [BindProperty(SupportsGet = true)]
    public long? GameId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Team1 { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Team2 { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool? CareAboutHomeAndAway { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public double? Spread { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool SpreadGreaterThanOrEqualTo { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public double? Total { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool TotalGreaterThanOrEqualTo { get; set; }

    public List<GameEntity> Games { get; set; } = new();

    private readonly IGamesService gamesService;

    public GameQuery(IGamesService gamesService) => this.gamesService = gamesService;
    
    public async Task OnGetAsync()
    {
        Games = await gamesService.GetGames(GameId, Team1, Team2, CareAboutHomeAndAway, Spread, 
            SpreadGreaterThanOrEqualTo, Total, TotalGreaterThanOrEqualTo);
    }
}