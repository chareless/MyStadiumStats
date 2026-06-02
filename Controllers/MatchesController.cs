using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;
using MyStadiumStats.Models.ViewModels;
using System.Text.Json;

namespace MyStadiumStats.Controllers;

public class MatchesController : Controller
{
    private readonly AppDbContext _db;
    public MatchesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var matches = await _db.Matches
            .Include(m => m.HomeTeam).Include(m => m.AwayTeam)
            .Include(m => m.Stadium).Include(m => m.Goals)
            .OrderByDescending(m => m.Date).ToListAsync();
        return View(matches);
    }

    public async Task<IActionResult> Details(int id)
    {
        var match = await _db.Matches
            .Include(m => m.HomeTeam).Include(m => m.AwayTeam)
            .Include(m => m.Stadium).Include(m => m.HomeCoach).Include(m => m.AwayCoach)
            .Include(m => m.Goals).ThenInclude(g => g.Player).ThenInclude(p => p.Team)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (match == null) return NotFound();
        return View(match);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new MatchFormViewModel();
        await PopulateFormViewModel(vm);
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MatchFormViewModel vm)
    {
        var goalInputs = DeserializeGoals(vm.GoalsJson);
        ValidateGoalScore(vm, goalInputs);
        if (!ModelState.IsValid)
        {
            await PopulateFormViewModel(vm);
            return View(vm);
        }

        var homeTeam  = await FindOrCreateTeam(vm.HomeTeamName);
        var awayTeam  = await FindOrCreateTeam(vm.AwayTeamName);
        var stadium   = await FindOrCreateStadium(vm.StadiumName);
        var homeCoach = await FindOrCreateCoach(vm.HomeCoachName);
        var awayCoach = await FindOrCreateCoach(vm.AwayCoachName);

        var match = new Match
        {
            Date = vm.Date, HomeTeamId = homeTeam.Id, AwayTeamId = awayTeam.Id,
            StadiumId = stadium.Id, HomeScore = vm.HomeScore, AwayScore = vm.AwayScore,
            MatchType = vm.MatchType, HomeCoachId = homeCoach?.Id, AwayCoachId = awayCoach?.Id,
            Notes = vm.Notes
        };
        _db.Matches.Add(match);
        await _db.SaveChangesAsync();
        await ProcessGoals(goalInputs, match.Id, homeTeam.Id, awayTeam.Id);
        return RedirectToAction(nameof(Details), new { id = match.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var match = await _db.Matches
            .Include(m => m.HomeTeam).Include(m => m.AwayTeam).Include(m => m.Stadium)
            .Include(m => m.HomeCoach).Include(m => m.AwayCoach)
            .Include(m => m.Goals).ThenInclude(g => g.Player)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (match == null) return NotFound();

        var existingGoals = match.Goals.OrderBy(g => g.Minute).Select(g => new GoalInput
        {
            PlayerName  = g.Player.Name,
            JerseyNumber = g.JerseyNumberAtMatch,
            Minute      = g.Minute,
            IsOwnGoal   = g.IsOwnGoal,
            IsPenalty   = g.IsPenalty,
            TeamSide    = g.Player.TeamId == match.HomeTeamId ? "home" : "away"
        }).ToList();

        var vm = new MatchFormViewModel
        {
            Id = match.Id, Date = match.Date,
            HomeTeamName = match.HomeTeam.Name, AwayTeamName = match.AwayTeam.Name,
            StadiumName  = match.Stadium.Name,
            HomeScore    = match.HomeScore, AwayScore  = match.AwayScore,
            MatchType    = match.MatchType,
            HomeCoachName = match.HomeCoach?.Name, AwayCoachName = match.AwayCoach?.Name,
            Notes        = match.Notes,
            GoalsJson    = JsonSerializer.Serialize(existingGoals)
        };
        await PopulateFormViewModel(vm);
        return View("Create", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MatchFormViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        var goalInputs = DeserializeGoals(vm.GoalsJson);
        ValidateGoalScore(vm, goalInputs);
        if (!ModelState.IsValid)
        {
            await PopulateFormViewModel(vm);
            return View("Create", vm);
        }

        var match = await _db.Matches.Include(m => m.Goals).FirstOrDefaultAsync(m => m.Id == id);
        if (match == null) return NotFound();

        var homeTeam  = await FindOrCreateTeam(vm.HomeTeamName);
        var awayTeam  = await FindOrCreateTeam(vm.AwayTeamName);
        var stadium   = await FindOrCreateStadium(vm.StadiumName);
        var homeCoach = await FindOrCreateCoach(vm.HomeCoachName);
        var awayCoach = await FindOrCreateCoach(vm.AwayCoachName);

        match.Date = vm.Date; match.HomeTeamId = homeTeam.Id; match.AwayTeamId = awayTeam.Id;
        match.StadiumId = stadium.Id; match.HomeScore = vm.HomeScore; match.AwayScore = vm.AwayScore;
        match.MatchType = vm.MatchType; match.HomeCoachId = homeCoach?.Id; match.AwayCoachId = awayCoach?.Id;
        match.Notes = vm.Notes;

        _db.Goals.RemoveRange(match.Goals);
        await _db.SaveChangesAsync();
        await ProcessGoals(goalInputs, match.Id, homeTeam.Id, awayTeam.Id);
        return RedirectToAction(nameof(Details), new { id = match.Id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var match = await _db.Matches.Include(m => m.Goals).FirstOrDefaultAsync(m => m.Id == id);
        if (match != null) { _db.Goals.RemoveRange(match.Goals); _db.Matches.Remove(match); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    // ---- Helpers ----
    private static List<GoalInput> DeserializeGoals(string json)
    {
        try { return JsonSerializer.Deserialize<List<GoalInput>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? []; }
        catch { return []; }
    }

    private void ValidateGoalScore(MatchFormViewModel vm, List<GoalInput> goals)
    {
        var valid = goals.Where(g => !string.IsNullOrWhiteSpace(g.PlayerName)).ToList();
        int hg = valid.Count(g => (g.TeamSide == "home" && !g.IsOwnGoal) || (g.TeamSide == "away" && g.IsOwnGoal));
        int ag = valid.Count(g => (g.TeamSide == "away" && !g.IsOwnGoal) || (g.TeamSide == "home" && g.IsOwnGoal));
        if (hg != vm.HomeScore || ag != vm.AwayScore)
            ModelState.AddModelError("", $"Gol sayıları skor ile uyuşmuyor. Ev Sahibi: {hg} gol girildi ama skor {vm.HomeScore}. Deplasman: {ag} gol girildi ama skor {vm.AwayScore}.");
    }

    private async Task ProcessGoals(List<GoalInput> goalInputs, int matchId, int homeTeamId, int awayTeamId)
    {
        foreach (var g in goalInputs.Where(g => !string.IsNullOrWhiteSpace(g.PlayerName)))
        {
            var teamId = g.TeamSide == "home" ? homeTeamId : awayTeamId;
            var name = g.PlayerName.Trim();
            // Load candidates into memory to avoid SQLite LOWER() failing on Turkish/Unicode chars
            var candidates = await _db.Players.Where(p => p.TeamId == teamId).ToListAsync();
            var player = candidates.FirstOrDefault(p =>
                string.Equals(p.Name.Trim(), name, StringComparison.OrdinalIgnoreCase));
            if (player == null)
            {
                player = new Player { Name = name, TeamId = teamId, CurrentJerseyNumber = g.JerseyNumber };
                _db.Players.Add(player);
                await _db.SaveChangesAsync();
            }
            else if (g.JerseyNumber.HasValue)
                player.CurrentJerseyNumber = g.JerseyNumber;

            _db.Goals.Add(new Goal
            {
                MatchId = matchId, PlayerId = player.Id, Minute = g.Minute,
                IsOwnGoal = g.IsOwnGoal, IsPenalty = g.IsPenalty, JerseyNumberAtMatch = g.JerseyNumber
            });
        }
        await _db.SaveChangesAsync();
    }

    private async Task<Team> FindOrCreateTeam(string name)
    {
        var n = name.Trim();
        var all = await _db.Teams.ToListAsync();
        var t = all.FirstOrDefault(x => string.Equals(x.Name.Trim(), n, StringComparison.OrdinalIgnoreCase));
        if (t == null) { t = new Team { Name = n }; _db.Teams.Add(t); await _db.SaveChangesAsync(); }
        return t;
    }

    private async Task<Stadium> FindOrCreateStadium(string name)
    {
        var n = name.Trim();
        var all = await _db.Stadiums.ToListAsync();
        var s = all.FirstOrDefault(x => string.Equals(x.Name.Trim(), n, StringComparison.OrdinalIgnoreCase));
        if (s == null) { s = new Stadium { Name = n }; _db.Stadiums.Add(s); await _db.SaveChangesAsync(); }
        return s;
    }

    private async Task<Coach?> FindOrCreateCoach(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        var n = name.Trim();
        var all = await _db.Coaches.ToListAsync();
        var c = all.FirstOrDefault(x => string.Equals(x.Name.Trim(), n, StringComparison.OrdinalIgnoreCase));
        if (c == null) { c = new Coach { Name = n }; _db.Coaches.Add(c); await _db.SaveChangesAsync(); }
        return c;
    }

    private async Task PopulateFormViewModel(MatchFormViewModel vm)
    {
        vm.TeamNames    = await _db.Teams.OrderBy(t => t.Name).Select(t => t.Name).ToListAsync();
        vm.StadiumNames = await _db.Stadiums.OrderBy(s => s.Name).Select(s => s.Name).ToListAsync();
        vm.CoachNames   = await _db.Coaches.OrderBy(c => c.Name).Select(c => c.Name).ToListAsync();
        vm.Players      = await _db.Players.Include(p => p.Team).OrderBy(p => p.Name).ToListAsync();
    }
}