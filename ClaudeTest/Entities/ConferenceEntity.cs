using System.ComponentModel.DataAnnotations.Schema;

namespace ClaudeTest.Entities;

[Table("Conferences")]
public class ConferenceEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;
}