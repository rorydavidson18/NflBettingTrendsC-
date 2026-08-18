using System.ComponentModel.DataAnnotations;

namespace ClaudeTest.Shared.Models;

public class UserBetInsertModel
{
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Bet size must be greater than 0")]
    public decimal BetSize { get; set; }

    public int Odds { get; set; }

    [Range(1, long.MaxValue, ErrorMessage = "GameId must reference a valid game")]
    public long GameId { get; set; }

    [Range(1, 50, ErrorMessage = "TeamId must reference a valid team")]
    public long TeamId { get; set; }
}
