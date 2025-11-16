using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

public class ExerciseTypeController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExerciseTypeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ExerciseType
    public async Task<IActionResult> Index()
    {
        return View(await _context.ExerciseTypes.ToListAsync());
    }

    // GET: ExerciseType/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exerciseType = await _context.ExerciseTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exerciseType == null)
        {
            return NotFound();
        }

        return View(exerciseType);
    }

    // GET: ExerciseType/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: ExerciseType/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] ExerciseType exerciseType)
    {
        if (ModelState.IsValid)
        {
            _context.Add(exerciseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(exerciseType);
    }

    // GET: ExerciseType/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exerciseType = await _context.ExerciseTypes.FindAsync(id);
        if (exerciseType == null)
        {
            return NotFound();
        }
        return View(exerciseType);
    }

    // POST: ExerciseType/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ExerciseType exerciseType)
    {
        if (id != exerciseType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exerciseType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseTypeExists(exerciseType.Id))
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
        return View(exerciseType);
    }

    // GET: ExerciseType/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exerciseType = await _context.ExerciseTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (exerciseType == null)
        {
            return NotFound();
        }

        return View(exerciseType);
    }

    // POST: ExerciseType/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var exerciseType = await _context.ExerciseTypes.FindAsync(id);
        if (exerciseType != null)
        {
            _context.ExerciseTypes.Remove(exerciseType);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ExerciseTypeExists(int id)
    {
        return _context.ExerciseTypes.Any(e => e.Id == id);
    }
}

