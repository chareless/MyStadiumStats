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
            .Include(m => m.Goals)
                .ThenInclude(g => g.TeamAtMatch)
            .AsNoTracking()
            .OrderByDescending(m => m.Date)
            .ToListAsync();

        // My Team stats
        int homeWins = 0, awayWins = 0, homeDraws = 0, awayDraws = 0, homeLosses = 0, awayLosses = 0, others = 0;

        foreach (var match in matches)
        {
            if (myTeam == null)
            {
                others++;
            }
            else if (match.HomeTeamId == myTeam.Id)
            {
                if (match.HomeScore > match.AwayScore) homeWins++;
                else if (match.HomeScore == match.AwayScore) homeDraws++;
                else homeLosses++;
            }
            else if (match.AwayTeamId == myTeam.Id)
            {
                if (match.AwayScore > match.HomeScore) awayWins++;
                else if (match.HomeScore == match.AwayScore) awayDraws++;
                else awayLosses++;
            }
            else
            {
                others++;
            }
        }

        var allGoals = matches.SelectMany(m => m.Goals).ToList();
        var totalGoals = allGoals.Where(g => !g.IsOwnGoal).Count();

        var topScorers = allGoals
            .Where(g => !g.IsOwnGoal)
            .GroupBy(g => g.Player.Name)
            .Select(g => new PlayerGoalStat
            {
                PlayerId = g.First().PlayerId,
                PlayerName = g.Key,
                TeamName = g.Select(x => x.TeamAtMatch?.Name).Distinct().Count() > 1 ? "Birden Fazla" : g.First().TeamAtMatch?.Name ?? "-",
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
                Breakdown = g.GroupBy(x => new { Team = x.TeamAtMatch?.Name, Jersey = x.JerseyNumberAtMatch })
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

        // Top scorers for my team
        var myTeamTotalGoals = myTeam != null 
            ? allGoals.Where(g => !g.IsOwnGoal && g.TeamIdAtMatch == myTeam.Id).Count()
            : 0;
        
        var topScorersForMyTeam = myTeam != null
            ? allGoals
                .Where(g => !g.IsOwnGoal && g.TeamIdAtMatch == myTeam.Id)
                .GroupBy(g => g.Player.Name)
                .Select(g => new PlayerGoalStat
                {
                    PlayerId = g.First().PlayerId,
                    PlayerName = g.Key,
                    TeamName = g.Select(x => x.TeamAtMatch?.Name).Distinct().Count() > 1 ? "Birden Fazla" : g.First().TeamAtMatch?.Name ?? "-",
                    TotalGoals = g.Count(),
                    PenaltyGoals = g.Count(x => x.IsPenalty),
                    GoalPercentage = myTeamTotalGoals > 0 ? Math.Round((double)g.Count() / myTeamTotalGoals * 100, 1) : 0,
                    JerseyNumbers = g.Where(x => x.JerseyNumberAtMatch.HasValue)
                                      .Select(x => x.JerseyNumberAtMatch!.Value)
                                      .Distinct()
                                      .OrderBy(n => n)
                                      .ToList(),
                    CurrentJerseyNumber = g.First().Player.CurrentJerseyNumber,
                    AvgGoalsPerMatch = Math.Round((double)g.Count() / g.Select(x => x.MatchId).Distinct().Count(), 2),
                    Breakdown = g.GroupBy(x => new { Team = x.TeamAtMatch?.Name, Jersey = x.JerseyNumberAtMatch })
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
                .ToList()
            : [];

        var goalsByMatchType = matches
            .GroupBy(m => m.MatchType)
            .ToDictionary(g => g.Key, g => g.Sum(m => m.Goals.Count));

        var matchesByType = matches
            .GroupBy(m => m.MatchType)
            .ToDictionary(g => g.Key, g => g.Count());

        // Wins and losses by match type for my team
        var winsAndLossesByType = new Dictionary<string, StatsByType>();
        if (myTeam != null)
        {
            var myMatches = matches.Where(m => m.HomeTeamId == myTeam.Id || m.AwayTeamId == myTeam.Id).ToList();
            foreach (var match in myMatches)
            {
                if (!winsAndLossesByType.ContainsKey(match.MatchType))
                {
                    winsAndLossesByType[match.MatchType] = new StatsByType { MatchType = match.MatchType };
                }

                bool myHome = match.HomeTeamId == myTeam.Id;
                bool win = myHome ? match.HomeScore > match.AwayScore : match.AwayScore > match.HomeScore;
                bool draw = match.HomeScore == match.AwayScore;

                winsAndLossesByType[match.MatchType].TotalMatches++;
                if (win) winsAndLossesByType[match.MatchType].Wins++;
                else if (draw) winsAndLossesByType[match.MatchType].Draws++;
                else winsAndLossesByType[match.MatchType].Losses++;
            }
        }

        // Wins and losses by stadium for my team
        var statsByStadium = new Dictionary<string, StadiumStat>();
        if (myTeam != null)
        {
            var myMatches = matches.Where(m => m.HomeTeamId == myTeam.Id || m.AwayTeamId == myTeam.Id).ToList();
            foreach (var match in myMatches)
            {
                string stadiumKey = match.Stadium.Name;
                if (!statsByStadium.ContainsKey(stadiumKey))
                {
                    statsByStadium[stadiumKey] = new StadiumStat { StadiumName = match.Stadium.Name };
                }

                bool myHome = match.HomeTeamId == myTeam.Id;
                bool win = myHome ? match.HomeScore > match.AwayScore : match.AwayScore > match.HomeScore;
                bool draw = match.HomeScore == match.AwayScore;

                statsByStadium[stadiumKey].TotalMatches++;
                if (win) statsByStadium[stadiumKey].Wins++;
                else if (draw) statsByStadium[stadiumKey].Draws++;
                else statsByStadium[stadiumKey].Losses++;
            }
        }

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
            HomeDraws = homeDraws,
            AwayDraws = awayDraws,
            HomeLosses = homeLosses,
            AwayLosses = awayLosses,
            OtherResults = others,
            TotalGoalsWitnessed = totalGoals,
            MyTeamTotalGoals = myTeamTotalGoals,
            RecentMatches = matches.Take(5).ToList(),
            TopScorers = topScorers.Take(10).ToList(),
            TopScorersForMyTeam = topScorersForMyTeam.Take(10).ToList(),
            GoalsByMatchType = goalsByMatchType,
            MatchesByType = matchesByType,
            WinsAndLossesByType = winsAndLossesByType,
            StatsByStadium = statsByStadium
        };

        return View(vm);
    }

    public IActionResult ClearAllData()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearAllData(string confirm)
    {
        if (confirm != "SILI") 
            return View();

        try
        {
            // Tüm Goals sil
            await _db.Goals.ExecuteDeleteAsync();
            
            // Tüm Matches sil
            await _db.Matches.ExecuteDeleteAsync();
            
            // Tüm Players sil
            await _db.Players.ExecuteDeleteAsync();
            
            // Tüm Coaches sil
            await _db.Coaches.ExecuteDeleteAsync();
            
            // Tüm Stadiums sil
            await _db.Stadiums.ExecuteDeleteAsync();
            
            // Teams'ı reset et (MyTeam ve Followed flags)
            var teams = await _db.Teams.ToListAsync();
            foreach (var team in teams)
            {
                team.IsMyTeam = false;
                team.IsFollowed = false;
            }
            await _db.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "✅ Bütün veriler başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"❌ Hata oluştu: {ex.Message}";
            return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
