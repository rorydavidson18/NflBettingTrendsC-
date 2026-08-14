using ClaudeTest.Interfaces;
using ClaudeTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClaudeTest.Pages.Games;

[Authorize(Policy = "AdminOnly")]
public class GamesInsert(IGamesService gamesService) : PageModel
{
    [BindProperty]
    public GameInsertModel Game { get; set; } = new ();

    public string? Message { get; set; }
    
    private readonly IGamesService gamesService = gamesService;

    public void OnGet() { }
    
    public async Task<IActionResult> OnPostAsync()
    {
        await gamesService.InsertGame(Game);
        
        Message = "Game successfully inserted";

        return RedirectToPage("/Games/GamesInsert");
    }
}