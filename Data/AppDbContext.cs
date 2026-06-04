using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Models;

namespace MyStadiumStats.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Team> Teams { get; set; }
    public DbSet<Stadium> Stadiums { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Goal> Goals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Match -> HomeTeam & AwayTeam (restrict cascade to avoid cycles)
        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany(t => t.HomeMatches)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany(t => t.AwayMatches)
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Match -> HomeCoach & AwayCoach (optional, set null on delete)
        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeCoach)
            .WithMany(c => c.HomeCoachMatches)
            .HasForeignKey(m => m.HomeCoachId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayCoach)
            .WithMany(c => c.AwayCoachMatches)
            .HasForeignKey(m => m.AwayCoachId)
            .OnDelete(DeleteBehavior.SetNull);

        // Goal -> TeamAtMatch (which team the goal was scored for)
        modelBuilder.Entity<Goal>()
            .HasOne(g => g.TeamAtMatch)
            .WithMany(t => t.GoalsForTeam)
            .HasForeignKey(g => g.TeamIdAtMatch)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
