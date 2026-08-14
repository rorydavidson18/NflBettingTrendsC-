using ClaudeTest.Entities;
using ClaudeTest.Models;

namespace ClaudeTest.Interfaces;

public interface IUserBetsService
{
    public Task InsertUserBet(UserBetInsertModel model, Guid oid);
    public Task<List<UserBetEntity>> GetUserBets(Guid oid);
}
