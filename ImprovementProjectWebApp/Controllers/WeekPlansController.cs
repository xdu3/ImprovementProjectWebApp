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
    public class WeekPlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeekPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WeekPlans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WeekPlan.Include(w => w.AppUserPlan).ThenInclude(w=>w.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WeekPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekPlan = await _context.WeekPlan
                .Include(w => w.AppUserPlan)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (weekPlan == null)
            {
                return NotFound();
            }

            return View(weekPlan);
        }

        // GET: WeekPlans/Create
        public IActionResult Create()
        {
            ViewData["AppUserPlanId"] = new SelectList(_context.AppUserPlans, "Id", "ApplicationUserId");
            return View();
        }

        // POST: WeekPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,WeekPlanStartTime,WeekPlanEndTime,AppUserPlanId")] WeekPlan weekPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weekPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserPlanId"] = new SelectList(_context.AppUserPlans, "Id", "ApplicationUserId", weekPlan.AppUserPlanId);
            return View(weekPlan);
        }

        // GET: WeekPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekPlan = await _context.WeekPlan.SingleOrDefaultAsync(m => m.Id == id);
            if (weekPlan == null)
            {
                return NotFound();
            }
            ViewData["AppUserPlanId"] = new SelectList(_context.AppUserPlans, "Id", "ApplicationUserId", weekPlan.AppUserPlanId);
            return View(weekPlan);
        }

        // POST: WeekPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,WeekPlanStartTime,WeekPlanEndTime,AppUserPlanId")] WeekPlan weekPlan)
        {
            if (id != weekPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weekPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeekPlanExists(weekPlan.Id))
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
            ViewData["AppUserPlanId"] = new SelectList(_context.AppUserPlans, "Id", "ApplicationUserId", weekPlan.AppUserPlanId);
            return View(weekPlan);
        }

        // GET: WeekPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weekPlan = await _context.WeekPlan
                .Include(w => w.AppUserPlan)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (weekPlan == null)
            {
                return NotFound();
            }

            return View(weekPlan);
        }

        // POST: WeekPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weekPlan = await _context.WeekPlan.SingleOrDefaultAsync(m => m.Id == id);
            _context.WeekPlan.Remove(weekPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeekPlanExists(int id)
        {
            return _context.WeekPlan.Any(e => e.Id == id);
        }
    }
}
