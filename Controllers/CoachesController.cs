using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;

namespace MyStadiumStats.Controllers;

public class CoachesController : Controller
{
    private readonly AppDbContext _db;

    public CoachesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var coaches = await _db.Coaches
            .Include(c => c.HomeCoachMatches).ThenInclude(m => m.HomeTeam)
            .Include(c => c.HomeCoachMatches).ThenInclude(m => m.AwayTeam)
            .Include(c => c.AwayCoachMatches).ThenInclude(m => m.HomeTeam)
            .Include(c => c.AwayCoachMatches).ThenInclude(m => m.AwayTeam)
            .OrderBy(c => c.Name)
            .ToListAsync();
        return View(coaches);
    }
}
