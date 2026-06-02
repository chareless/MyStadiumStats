using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;
using MyStadiumStats.Models.ViewModels;
using System.Diagnostics;

namespace MyStadiumStats.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var myTeam = await _db.Teams.FirstOrDefaultAsync(t => t.IsMyTeam);
        var followedTeams = await _db.Teams.Where(t => t.IsFollowed).OrderBy(t => t.Name).ToListAsync();

        var matches = await _db.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Stadium)
            .Include(m => m.Goals)
                .ThenInclude(g => g.Player)
                    .ThenInclude(p => p.Team)
            .OrderByDescending(m => m.Date)
            .ToListAsync();

        // My Team stats
        int homeWins = 0, awayWins = 0, draws = 0, losses = 0, others = 0;

        foreach (var match in matches)
        {
            if (myTeam == null)
            {
                others++;
            }
            else if (match.HomeTeamId == myTeam.Id)
            {
                if (match.HomeScore > match.AwayScore) homeWins++;
                else if (match.HomeScore == match.AwayScore) draws++;
                else losses++;
            }
            else if (match.AwayTeamId == myTeam.Id)
            {
                if (match.AwayScore > match.HomeScore) awayWins++;
                else if (match.HomeScore == match.AwayScore) draws++;
                else losses++;
            }
            else
            {
                others++;
            }
        }

        var allGoals = matches.SelectMany(m => m.Goals).ToList();
        var totalGoals = allGoals.Count;

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
                GoalPercentage = totalGoals > 0 ? Math.Round((double)g.Count() / totalGoals * 100, 1) : 0,
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

        var goalsByMatchType = matches
            .GroupBy(m => m.MatchType)
            .ToDictionary(g => g.Key, g => g.Sum(m => m.Goals.Count));

        var matchesByType = matches
            .GroupBy(m => m.MatchType)
            .ToDictionary(g => g.Key, g => g.Count());

        // Followed teams statistics
        var followedTeamsStats = new List<DashboardViewModel.FollowedTeamStat>();
        foreach (var team in followedTeams)
        {
            var teamMatches = matches.Where(m => m.HomeTeamId == team.Id || m.AwayTeamId == team.Id).ToList();
            int tWins = 0, tDraws = 0, tLosses = 0;
            foreach (var match in teamMatches)
            {
                if (match.HomeTeamId == team.Id)
                {
                    if (match.HomeScore > match.AwayScore) tWins++;
                    else if (match.HomeScore == match.AwayScore) tDraws++;
                    else tLosses++;
                }
                else
                {
                    if (match.AwayScore > match.HomeScore) tWins++;
                    else if (match.HomeScore == match.AwayScore) tDraws++;
                    else tLosses++;
                }
            }
            followedTeamsStats.Add(new DashboardViewModel.FollowedTeamStat
            {
                Team = team,
                Wins = tWins,
                Draws = tDraws,
                Losses = tLosses,
                TotalMatches = teamMatches.Count,
                RecentMatches = teamMatches.Take(5).ToList()
            });
        }

        var vm = new DashboardViewModel
        {
            MyTeam = myTeam,
            FollowedTeams = followedTeamsStats,
            TotalMatches = matches.Count,
            HomeWins = homeWins,
            AwayWins = awayWins,
            Draws = draws,
            Losses = losses,
            OtherResults = others,
            TotalGoalsWitnessed = totalGoals,
            RecentMatches = matches.Take(5).ToList(),
            TopScorers = topScorers.Take(10).ToList(),
            GoalsByMatchType = goalsByMatchType,
            MatchesByType = matchesByType
        };

        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
