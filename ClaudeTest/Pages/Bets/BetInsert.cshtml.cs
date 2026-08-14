using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using ClaudeTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

namespace ClaudeTest.Pages.Bets;

[Authorize(Policy = "BetTracker")]
public class BetInsert(IUserBetsService userBetsService) : PageModel
{
    [BindProperty]
    public UserBetInsertModel Bet { get; set; } = new();

    public string? Message { get; set; }

    public List<UserBetEntity> Bets { get; set; } = [];

    public decimal TotalPayout => Bets.Sum(b => b.Payout);

    private readonly IUserBetsService userBetsService = userBetsService;

    public async Task OnGet()
    {
        var oid = Guid.Parse(User.GetObjectId()!);
        Bets = await userBetsService.GetUserBets(oid);
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

        Bets = await userBetsService.GetUserBets(oid);

        return Page();
    }
}
