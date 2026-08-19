using NflBettingTrends.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NflBettingTrends.Shared.Entities;
using Microsoft.AspNetCore.Authorization;

namespace NflBettingTrends.Pages.Teams;

[Authorize(Policy = "AdminOnly")]
public class UpsertModel : PageModel
{
    private readonly NflDbContext _db;
    public UpsertModel(NflDbContext db) => _db = db;

    [BindProperty]
    public TeamEntity Team { get; set; } = new();

    public SelectList Divisions { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id.HasValue)
        {
            var found = await _db.Teams.FindAsync(id.Value);
            if (found is null) return NotFound();
            Team = found;
        }

        await LoadDivisionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDivisionsAsync();
            return Page();
        }

        if (Team.Id == 0)
            _db.Teams.Add(Team);         // INSERT
        else
            _db.Teams.Update(Team);      // UPDATE

        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }

    private async Task LoadDivisionsAsync()
    {
        var divisions = await _db.Divisions
            .Include(d => d.Conference)
            .OrderBy(d => d.Conference.Name)
            .ThenBy(d => d.Name)
            .Select(d => new { d.Id, d.Name })
            .ToListAsync();

        Divisions = new SelectList(divisions, "Id", "Name", Team.DivisionId);
    }
}