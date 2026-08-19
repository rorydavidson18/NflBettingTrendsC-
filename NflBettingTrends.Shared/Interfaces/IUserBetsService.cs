using NflBettingTrends.Shared.Entities;
using NflBettingTrends.Shared.Models;

namespace NflBettingTrends.Shared.Interfaces;

public interface IUserBetsService
{
    public Task InsertUserBet(UserBetInsertModel model, Guid oid);
    public Task<List<UserBetEntity>> GetUserBets(Guid oid);
    public Task<List<Guid>> GetDistinctUserIds();
}
