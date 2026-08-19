using System.ComponentModel.DataAnnotations.Schema;

namespace NflBettingTrends.Shared.Entities;

[Table("Divisions")]
public class DivisionEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;
 
    public long ConferenceId { get; set; }

    [ForeignKey("ConferenceId")] 
    public ConferenceEntity Conference { get; set; } = null!;
}