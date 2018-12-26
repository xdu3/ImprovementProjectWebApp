using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;

namespace ImprovementProjectWebApp.Controllers
{
    public class RepsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reps.Include(r => r.WorkoutPlan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reps = await _context.Reps
                .Include(r => r.WorkoutPlan)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reps == null)
            {
                return NotFound();
            }

            return View(reps);
        }

        // GET: Reps/Create
        public IActionResult Create()
        {
            ViewData["WorkoutPlanId"] = new SelectList(_context.WorkoutPlan, "Id", "PlanName");
            return View();
        }

        // POST: Reps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,num,WorkoutPlanId")] Reps reps)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reps);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkoutPlanId"] = new SelectList(_context.WorkoutPlan, "Id", "PlanName", reps.WorkoutPlanId);
            return View(reps);
        }

        // GET: Reps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reps = await _context.Reps.SingleOrDefaultAsync(m => m.Id == id);
            if (reps == null)
            {
                return NotFound();
            }
            ViewData["WorkoutPlanId"] = new SelectList(_context.WorkoutPlan, "Id", "PlanName", reps.WorkoutPlanId);
            return View(reps);
        }

        // POST: Reps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,num,WorkoutPlanId")] Reps reps)
        {
            if (id != reps.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reps);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepsExists(reps.Id))
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
            ViewData["WorkoutPlanId"] = new SelectList(_context.WorkoutPlan, "Id", "PlanName", reps.WorkoutPlanId);
            return View(reps);
        }

        // GET: Reps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reps = await _context.Reps
                .Include(r => r.WorkoutPlan)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (reps == null)
            {
                return NotFound();
            }

            return View(reps);
        }

        // POST: Reps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reps = await _context.Reps.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reps.Remove(reps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepsExists(int id)
        {
            return _context.Reps.Any(e => e.Id == id);
        }
    }
}
