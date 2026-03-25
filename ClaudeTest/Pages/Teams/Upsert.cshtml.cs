using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClaudeTest.Data;
using ClaudeTest.Entities;

namespace ClaudeTest.Pages.Teams;

public class UpsertModel : PageModel
{
    private readonly NflDbContext _db;
    public UpsertModel(NflDbContext db) => _db = db;

    [BindProperty]
    public TeamEntity Team { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id.HasValue)
        {
            var found = await _db.Teams.FindAsync(id.Value);
            if (found is null) return NotFound();
            Team = found;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (Team.Id == 0)
            _db.Teams.Add(Team);         // INSERT
        else
            _db.Teams.Update(Team);      // UPDATE

        await _db.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}