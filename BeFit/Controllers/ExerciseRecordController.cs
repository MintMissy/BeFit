using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

[Authorize]
public class ExerciseRecordController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ExerciseRecordController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: ExerciseRecord
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var exerciseRecords = await _context.ExerciseRecords
            .Include(er => er.TrainingSession)
            .Include(er => er.ExerciseType)
            .Where(er => er.TrainingSession.UserId == userId)
            .OrderByDescending(er => er.TrainingSession.StartDateTime)
            .ToListAsync();
        return View(exerciseRecords);
    }

    // GET: ExerciseRecord/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var exerciseRecord = await _context.ExerciseRecords
            .Include(er => er.ExerciseType)
            .Include(er => er.TrainingSession)
            .FirstOrDefaultAsync(m => m.Id == id && m.TrainingSession.UserId == userId);
        
        if (exerciseRecord == null)
        {
            return NotFound();
        }

        return View(exerciseRecord);
    }

    // GET: ExerciseRecord/Create
    public async Task<IActionResult> Create()
    {
        var userId = _userManager.GetUserId(User);
        ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
        ViewData["TrainingSessionId"] = new SelectList(
            await _context.TrainingSessions
                .Where(ts => ts.UserId == userId)
                .OrderByDescending(ts => ts.StartDateTime)
                .ToListAsync(),
            "Id",
            "StartDateTime");
        return View();
    }

    // POST: ExerciseRecord/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,TrainingSessionId,ExerciseTypeId,Weight,Sets,Repetitions,Notes")] ExerciseRecord exerciseRecord)
    {
        var userId = _userManager.GetUserId(User);
        
        // Remove navigation properties from ModelState since they're set automatically by EF Core
        ModelState.Remove(nameof(ExerciseRecord.TrainingSession));
        ModelState.Remove(nameof(ExerciseRecord.ExerciseType));
        
        // Verify that the training session belongs to the current user
        var trainingSession = await _context.TrainingSessions
            .FirstOrDefaultAsync(ts => ts.Id == exerciseRecord.TrainingSessionId && ts.UserId == userId);
        
        if (trainingSession == null)
        {
            ModelState.AddModelError("TrainingSessionId", "Nieprawidłowa sesja treningowa");
        }
        
        if (ModelState.IsValid)
        {
            _context.Add(exerciseRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseRecord.ExerciseTypeId);
        ViewData["TrainingSessionId"] = new SelectList(
            await _context.TrainingSessions
                .Where(ts => ts.UserId == userId)
                .OrderByDescending(ts => ts.StartDateTime)
                .ToListAsync(),
            "Id",
            "StartDateTime",
            exerciseRecord.TrainingSessionId);
        return View(exerciseRecord);
    }

    // GET: ExerciseRecord/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var exerciseRecord = await _context.ExerciseRecords
            .Include(er => er.TrainingSession)
            .FirstOrDefaultAsync(er => er.Id == id && er.TrainingSession.UserId == userId);
        
        if (exerciseRecord == null)
        {
            return NotFound();
        }
        
        ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseRecord.ExerciseTypeId);
        ViewData["TrainingSessionId"] = new SelectList(
            await _context.TrainingSessions
                .Where(ts => ts.UserId == userId)
                .OrderByDescending(ts => ts.StartDateTime)
                .ToListAsync(),
            "Id",
            "StartDateTime",
            exerciseRecord.TrainingSessionId);
        return View(exerciseRecord);
    }

    // POST: ExerciseRecord/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,TrainingSessionId,ExerciseTypeId,Weight,Sets,Repetitions,Notes")] ExerciseRecord exerciseRecord)
    {
        if (id != exerciseRecord.Id)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        
        ModelState.Remove(nameof(ExerciseRecord.TrainingSession));
        ModelState.Remove(nameof(ExerciseRecord.ExerciseType));
        
        var originalRecord = await _context.ExerciseRecords
            .Include(er => er.TrainingSession)
            .AsNoTracking()
            .FirstOrDefaultAsync(er => er.Id == id && er.TrainingSession.UserId == userId);
        
        if (originalRecord == null)
        {
            return NotFound();
        }
        
        var trainingSession = await _context.TrainingSessions
            .FirstOrDefaultAsync(ts => ts.Id == exerciseRecord.TrainingSessionId && ts.UserId == userId);
        
        if (trainingSession == null)
        {
            ModelState.AddModelError("TrainingSessionId", "Nieprawidłowa sesja treningowa");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exerciseRecord);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseRecordExists(exerciseRecord.Id))
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
        
        ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseRecord.ExerciseTypeId);
        ViewData["TrainingSessionId"] = new SelectList(
            await _context.TrainingSessions
                .Where(ts => ts.UserId == userId)
                .OrderByDescending(ts => ts.StartDateTime)
                .ToListAsync(),
            "Id",
            "StartDateTime",
            exerciseRecord.TrainingSessionId);
        return View(exerciseRecord);
    }

    // GET: ExerciseRecord/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var exerciseRecord = await _context.ExerciseRecords
            .Include(er => er.ExerciseType)
            .Include(er => er.TrainingSession)
            .FirstOrDefaultAsync(m => m.Id == id && m.TrainingSession.UserId == userId);
        
        if (exerciseRecord == null)
        {
            return NotFound();
        }

        return View(exerciseRecord);
    }

    // POST: ExerciseRecord/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = _userManager.GetUserId(User);
        var exerciseRecord = await _context.ExerciseRecords
            .Include(er => er.TrainingSession)
            .FirstOrDefaultAsync(er => er.Id == id && er.TrainingSession.UserId == userId);
        
        if (exerciseRecord != null)
        {
            _context.ExerciseRecords.Remove(exerciseRecord);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ExerciseRecordExists(int id)
    {
        return _context.ExerciseRecords.Any(e => e.Id == id);
    }
}

