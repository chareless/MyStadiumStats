using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;

namespace MyStadiumStats.Controllers;

public class StadiumsController : Controller
{
    private readonly AppDbContext _db;

    public StadiumsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var stadiums = await _db.Stadiums
            .Include(s => s.Matches).ThenInclude(m => m.HomeTeam)
            .Include(s => s.Matches).ThenInclude(m => m.AwayTeam)
            .Include(s => s.Matches).ThenInclude(m => m.Goals)
            .OrderBy(s => s.Name)
            .ToListAsync();
        return View(stadiums);
    }
}
