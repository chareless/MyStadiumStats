using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStadiumStats.Data;
using MyStadiumStats.Models;

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
}