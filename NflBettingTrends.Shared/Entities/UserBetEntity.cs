using System.ComponentModel.DataAnnotations.Schema;

namespace NflBettingTrends.Shared.Entities;

[Table("userBets")]
public class UserBetEntity
{
    public long Id { get; set; }

    public Guid Oid { get; set; }
    
    public DateTime Date { get; set; }
    
    public decimal BetSize { get; set; }
    
    public double Odds { get; set; }
    
    public long GameId { get; set; }

    [ForeignKey("GameId")] 
    public GameEntity Game { get; set; } = null!;
    
    public decimal Payout { get; set; }

    public long? TeamId { get; set; }

    [ForeignKey("TeamId")] 
    public TeamEntity? Team { get; set; } = null!;
}