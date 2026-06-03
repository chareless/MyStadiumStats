using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;
using MyStadiumStats.Models.ViewModels;

namespace MyStadiumStats.Controllers;

public class PlayersController : Controller
{
    private readonly AppDbContext _db;
    public PlayersController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var players = await _db.Players
            .Include(p => p.Team)
            .Include(p => p.Goals)
            .OrderBy(p => p.Team!.Name).ThenBy(p => p.Name)
            .ToListAsync();
        return View(players);
    }

    public async Task<IActionResult> Statistics(int? teamId = null)
    {
        var matches = await _db.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Goals)
                .ThenInclude(g => g.Player)
                    .ThenInclude(p => p.Team)
            .ToListAsync();

        var allGoals = matches.SelectMany(m => m.Goals).ToList();
        
        // Filter by team if specified
        if (teamId.HasValue)
        {
            allGoals = allGoals.Where(g => g.Player.TeamId == teamId).ToList();
        }
        
        var totalGoals = allGoals.Where(g => !g.IsOwnGoal).Count();

        var topScorers = allGoals
            .Where(g => !g.IsOwnGoal)
            .GroupBy(g => g.PlayerId)
            .Select(g => new PlayerGoalStat
            {
                PlayerId = g.Key,
                PlayerName = g.First().Player.Name,
                TeamName = g.First().Player.Team?.Name,
                TotalGoals = g.Count(),
                PenaltyGoals = g.Count(x => x.IsPenalty),
                GoalPercentage = totalGoals > 0 ? Math.Round((double)g.Count() / totalGoals * 100, 2) : 0,
                JerseyNumbers = g.Where(x => x.JerseyNumberAtMatch.HasValue)
                                  .Select(x => x.JerseyNumberAtMatch!.Value)
                                  .Distinct()
                                  .OrderBy(n => n)
                                  .ToList(),
                CurrentJerseyNumber = g.First().Player.CurrentJerseyNumber,
                AvgGoalsPerMatch = Math.Round((double)g.Count() / g.Select(x => x.MatchId).Distinct().Count(), 2),
                Breakdown = g.GroupBy(x => new { Team = x.Player.Team?.Name, Jersey = x.JerseyNumberAtMatch })
                              .Select(bg => new PlayerGoalStat.GoalBreakdown
                              {
                                  TeamName = bg.Key.Team,
                                  JerseyNumber = bg.Key.Jersey,
                                  Goals = bg.Count(),
                                  PenaltyGoals = bg.Count(x => x.IsPenalty)
                              })
                              .OrderByDescending(b => b.Goals)
                              .ToList()
            })
            .OrderByDescending(p => p.TotalGoals)
            .ToList();
        
        // Pass team info to view
        if (teamId.HasValue)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            ViewData["TeamName"] = team?.Name;
        }

        return View(topScorers);
    }
}