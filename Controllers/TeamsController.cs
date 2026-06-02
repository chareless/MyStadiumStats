using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;

namespace MyStadiumStats.Controllers;

public class TeamsController : Controller
{
    private readonly AppDbContext _db;

    public TeamsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var teams = await _db.Teams
            .Include(t => t.HomeMatches).ThenInclude(m => m.Goals)
            .Include(t => t.AwayMatches).ThenInclude(m => m.Goals)
            .OrderBy(t => t.Name)
            .ToListAsync();
        return View(teams);
    }

    public IActionResult Create() => View(new Team());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Team team)
    {
        if (!ModelState.IsValid) return View(team);

        if (team.IsMyTeam)
        {
            var others = await _db.Teams.Where(t => t.IsMyTeam).ToListAsync();
            others.ForEach(t => t.IsMyTeam = false);
        }

        _db.Teams.Add(team);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var team = await _db.Teams.FindAsync(id);
        if (team == null) return NotFound();
        return View(team);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Team team)
    {
        if (id != team.Id) return NotFound();
        if (!ModelState.IsValid) return View(team);

        if (team.IsMyTeam)
        {
            var others = await _db.Teams.Where(t => t.IsMyTeam && t.Id != id).ToListAsync();
            others.ForEach(t => t.IsMyTeam = false);
        }

        _db.Teams.Update(team);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var team = await _db.Teams
            .Include(t => t.HomeMatches)
            .Include(t => t.AwayMatches)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (team != null)
        {
            if (team.HomeMatches.Any() || team.AwayMatches.Any())
            {
                TempData["Error"] = $"'{team.Name}' takımının maç kaydı olduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }
            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
