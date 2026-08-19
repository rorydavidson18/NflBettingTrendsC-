using NflBettingTrends.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace NflBettingTrends.Shared.Data;

public class NflDbContext : DbContext
{
    public NflDbContext(DbContextOptions<NflDbContext> options)
        : base(options)
    {
    }

    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<GameEntity> Games { get; set; }
    public DbSet<ConferenceEntity> Conferences { get; set; }
    public DbSet<DivisionEntity> Divisions { get; set; }
    public DbSet<UserBetEntity> UserBets { get; set; }
}