using System.ComponentModel.DataAnnotations.Schema;

namespace ClaudeTest.Shared.Entities;

[Table("games")]
public class GameEntity
{
    public long Id { get; set; }
    
    public long HomeTeamId { get; set; }

    [ForeignKey("HomeTeamId")] 
    public TeamEntity HomeTeam { get; set; } = null!;
    
    public long AwayTeamId { get; set; }
    
    [ForeignKey("AwayTeamId")]
    public TeamEntity AwayTeam { get; set; } = null!;
    
    public DateTime Date { get; set; }
    
    public int HomeScore { get; set; }
    
    public int AwayScore { get; set; }
    
    public double Spread { get; set; }

    [NotMapped]
    public double SpreadResult => (HomeScore - AwayScore) + Spread;
    
    [NotMapped]
    public string SpreadResultString => SpreadResult >= 0 ? 
        $"{HomeTeam.Abbreviation} covered by {SpreadResult}" : 
        $"{AwayTeam.Abbreviation} covered by {Math.Abs(SpreadResult)}";
    
    public double Total { get; set; }
    
    [NotMapped]
    public double TotalResult => (HomeScore + AwayScore) - Total;
    
    [NotMapped]
    public string TotalResultString => TotalResult >= 0 ? 
        $"Over hit by {TotalResult}" : 
        $"Under hit by {Math.Abs(TotalResult)}";
    
    public bool? PlayoffGame { get; set; }
}