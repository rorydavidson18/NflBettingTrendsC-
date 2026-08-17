using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using ClaudeTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace ClaudeTest.Pages.Bets;

[Authorize(Policy = "BetTracker")]
public class BetInsert : PageModel
{
    [BindProperty]
    public UserBetInsertModel Bet { get; set; } = new();

    public string? Message { get; set; }

    public List<UserBetEntity> Bets { get; set; } = [];

    public List<SelectListItem> Teams { get; set; } = new();

    public List<GameSelectItem> Games { get; set; } = new();

    public decimal TotalPayout => Bets.Sum(b => b.Payout);

    private readonly IUserBetsService userBetsService;
    private readonly ITeamsService teamsService;
    private readonly IGamesService gamesService;

    public BetInsert(IUserBetsService userBetsService, ITeamsService teamsService, IGamesService gamesService)
    {
        this.userBetsService = userBetsService;
        this.teamsService = teamsService;
        this.gamesService = gamesService;
    }

    public async Task OnGet()
    {
        var oid = Guid.Parse(User.GetObjectId()!);
        Bets = await userBetsService.GetUserBets(oid);
        await LoadTeams();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var oid = Guid.Parse(User.GetObjectId()!);

        if (ModelState.IsValid)
        {
            try
            {
                await userBetsService.InsertUserBet(Bet, oid);
                Message = "Bet successfully placed";
                Bet = new UserBetInsertModel();
                ModelState.Clear();
            }
            catch (DbUpdateException)
            {
                Message = "Could not place bet — GameId does not match an existing game.";
            }
        }
        else
        {
            await LoadTeams();
        }

        Bets = await userBetsService.GetUserBets(oid);

        return Page();
    }

    public async Task LoadTeams()
    {
        Teams = (await teamsService.GetAllTeams())
            .OrderBy(e => e.Abbreviation)
            .Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Abbreviation
            })
            .ToList();

        Games = (await gamesService.GetGames())
            .OrderByDescending(g => g.Date)
            .Select(g => new GameSelectItem()
            {
                Value = g.Id.ToString(),
                Text = $"{g.Date:MM/dd/yyyy} — {g.HomeTeam.Abbreviation} vs {g.AwayTeam.Abbreviation}",
                HomeTeamId = g.HomeTeamId.ToString(),
                AwayTeamId = g.AwayTeamId.ToString()
            })
            .ToList();
    }
}
