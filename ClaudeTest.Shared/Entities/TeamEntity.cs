using System.ComponentModel.DataAnnotations.Schema;

namespace ClaudeTest.Shared.Entities;

[Table("teams")]
public class TeamEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public long DivisionId { get; set; }

    [ForeignKey("DivisionId")] 
    public DivisionEntity Division { get; set; } = null!;

    // public List<GameEntity> Games { get; set; } = new();
}