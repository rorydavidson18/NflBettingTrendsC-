using ClaudeTest.Shared.Entities;
using ClaudeTest.Shared.Models;

namespace ClaudeTest.Shared.Interfaces;

public interface IUserBetsService
{
    public Task InsertUserBet(UserBetInsertModel model, Guid oid);
    public Task<List<UserBetEntity>> GetUserBets(Guid oid);
    public Task<List<Guid>> GetDistinctUserIds();
}
