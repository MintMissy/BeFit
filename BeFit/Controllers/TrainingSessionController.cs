using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

[Authorize]
public class TrainingSessionController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TrainingSessionController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: TrainingSession
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var trainingSessions = await _context.TrainingSessions
            .Where(ts => ts.UserId == userId)
            .OrderByDescending(ts => ts.StartDateTime)
            .ToListAsync();
        return View(trainingSessions);
    }

    // GET: TrainingSession/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var trainingSession = await _context.TrainingSessions
            .Include(ts => ts.ExerciseRecords)
            .ThenInclude(er => er.ExerciseType)
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        
        if (trainingSession == null)
        {
            return NotFound();
        }

        return View(trainingSession);
    }

    // GET: TrainingSession/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TrainingSession/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,StartDateTime,EndDateTime,Notes")] TrainingSession trainingSession)
    {
        trainingSession.UserId = _userManager.GetUserId(User)!;
        
        ModelState.Remove(nameof(TrainingSession.UserId));
        ModelState.Remove(nameof(TrainingSession.User));
        
        if (trainingSession.EndDateTime <= trainingSession.StartDateTime)
        {
            ModelState.AddModelError("EndDateTime", "Data zakończenia musi być późniejsza niż data rozpoczęcia");
        }
        
        if (ModelState.IsValid)
        {
            _context.Add(trainingSession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(trainingSession);
    }

    // GET: TrainingSession/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var trainingSession = await _context.TrainingSessions
            .FirstOrDefaultAsync(ts => ts.Id == id && ts.UserId == userId);
        
        if (trainingSession == null)
        {
            return NotFound();
        }
        return View(trainingSession);
    }

    // POST: TrainingSession/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StartDateTime,EndDateTime,Notes")] TrainingSession trainingSession)
    {
        if (id != trainingSession.Id)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var existingSession = await _context.TrainingSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(ts => ts.Id == id && ts.UserId == userId);
        
        if (existingSession == null)
        {
            return NotFound();
        }

        trainingSession.UserId = existingSession.UserId;
        
        ModelState.Remove(nameof(TrainingSession.UserId));
        ModelState.Remove(nameof(TrainingSession.User));
        
        if (trainingSession.EndDateTime <= trainingSession.StartDateTime)
        {
            ModelState.AddModelError("EndDateTime", "Data zakończenia musi być późniejsza niż data rozpoczęcia");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(trainingSession);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(trainingSession.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(trainingSession);
    }

    // GET: TrainingSession/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var trainingSession = await _context.TrainingSessions
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        
        if (trainingSession == null)
        {
            return NotFound();
        }

        return View(trainingSession);
    }

    // POST: TrainingSession/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = _userManager.GetUserId(User);
        var trainingSession = await _context.TrainingSessions
            .FirstOrDefaultAsync(ts => ts.Id == id && ts.UserId == userId);
        
        if (trainingSession != null)
        {
            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool TrainingSessionExists(int id)
    {
        return _context.TrainingSessions.Any(e => e.Id == id);
    }
}

