namespace NflBettingTrends.Models;

public class GameInsertModel
{
    public long Id { get; set; }

    public string HomeTeamAbv { get; set; } = null!;

    public string AwayTeamAbv { get; set; } = null!;
    
    public DateTime Date { get; set; }
    
    public int HomeScore { get; set; }
    
    public int AwayScore { get; set; }
    
    public double Spread { get; set; }
    
    public double Total { get; set; }
    
    public bool PlayoffGame { get; set; }
}