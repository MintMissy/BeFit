using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

[Authorize]
public class StatisticsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public StatisticsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Statistics
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var fourWeeksAgo = DateTime.Now.AddDays(-28);

        var exerciseRecords = await _context.ExerciseRecords
            .Include(er => er.ExerciseType)
            .Include(er => er.TrainingSession)
            .Where(er => er.TrainingSession.UserId == userId && 
                        er.TrainingSession.StartDateTime >= fourWeeksAgo)
            .ToListAsync();

        var statistics = exerciseRecords
            .GroupBy(er => new { er.ExerciseTypeId, er.ExerciseType.Name })
            .Select(g => new ExerciseStatistics
            {
                ExerciseName = g.Key.Name,
                TimesPerformed = g.Count(),
                TotalRepetitions = g.Sum(er => er.Sets * er.Repetitions),
                AverageWeight = g.Average(er => er.Weight),
                MaxWeight = g.Max(er => er.Weight)
            })
            .OrderByDescending(s => s.TimesPerformed)
            .ToList();

        ViewData["StartDate"] = fourWeeksAgo.ToString("dd.MM.yyyy");
        ViewData["EndDate"] = DateTime.Now.ToString("dd.MM.yyyy");

        return View(statistics);
    }
}

