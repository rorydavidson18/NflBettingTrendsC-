using NflBettingTrends.Interfaces;
using NflBettingTrends.Models;
using NflBettingTrends.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NflBettingTrends.Pages.Games;

[AllowAnonymous]
public class GameQuery(IGamesService gamesService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public long? GameId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Team1 { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Team2 { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool CareAboutHomeAndAway { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public double? Spread { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool SpreadGreaterThanOrEqualTo { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public double? Total { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public bool TotalGreaterThanOrEqualTo { get; set; }

    [BindProperty(SupportsGet = true)] 
    public DateTime StartDate { get; set; } = DateTime.Parse("08/31/2020");

    [BindProperty(SupportsGet = true)]
    public DateTime EndDate { get; set; } = DateTime.Now;

    public List<GameEntity> Games { get; set; } = [];

    public TrendsModel Trends { get; set; } = new();

    public List<AtsBinModel> AtsBins { get; set; } = [];

    private readonly IGamesService gamesService = gamesService;

    public async Task OnGetAsync()
    {
        Games = await gamesService.GetGames(GameId, Team1, Team2, CareAboutHomeAndAway, Spread,
            SpreadGreaterThanOrEqualTo, Total, TotalGreaterThanOrEqualTo, StartDate, EndDate);

        Trends = gamesService.CalculateTrends(Games);
        AtsBins = gamesService.CalculateAtsBins(Games);
    }
}