using System.ComponentModel.DataAnnotations;
using ClaudeTest.Entities;
using ClaudeTest.Interfaces;
using ClaudeTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClaudeTest.Pages.Games;

public class GamesInsert(IGamesService gamesService) : PageModel
{
    [BindProperty]
    [Required]
    public string HomeTeamAbv { get; set; } = null!;
    
    [BindProperty]
    [Required]
    public string AwayTeamAbv { get; set; } = null!;
    
    [BindProperty]
    [Required]
    public DateTime Date { get; set; }
    
    [BindProperty]
    [Required]
    public int HomeScore { get; set; }
    
    [BindProperty]
    [Required]
    public int AwayScore { get; set; }
    
    [BindProperty]
    [Required]
    public double Spread { get; set; }

    [BindProperty]
    [Required]
    public double Total { get; set; }
    
    [BindProperty]
    [Required]
    public bool PlayoffGame { get; set; }

    public string? Message { get; set; }
    
    private readonly IGamesService gamesService = gamesService;

    public async Task<PageResult> OnPostAsync()
    {
        var gameModel = new GameInsertModel()
        {
            HomeTeamAbv = HomeTeamAbv,
            AwayTeamAbv = AwayTeamAbv,
            Spread = Spread,
            Total = Total,
            HomeScore = HomeScore,
            AwayScore = AwayScore,
            PlayoffGame = PlayoffGame,
            Date = Date
        };
        
        await gamesService.InsertGame(gameModel);
        
        Message = "Game successfully inserted";

        return Page();
    }
}