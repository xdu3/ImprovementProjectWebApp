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
    public class AppUserPlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppUserPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppUserPlans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AppUserPlans.Include(a => a.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AppUserPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUserPlan = await _context.AppUserPlans
                .Include(a => a.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appUserPlan == null)
            {
                return NotFound();
            }

            return View(appUserPlan);
        }

        // GET: AppUserPlans/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser.Where(a=>a.IfDelete==false&& a.EmailConfirmed == true), "Id", "Email");
            return View();
        }

        // POST: AppUserPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,Price,IfLock,ApplicationUserId")] AppUserPlan appUserPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUserPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true), "Id", "Email", appUserPlan.ApplicationUserId);
            return View(appUserPlan);
        }

        // GET: AppUserPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUserPlan = await _context.AppUserPlans.SingleOrDefaultAsync(m => m.Id == id);
            if (appUserPlan == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true), "Id", "Email", appUserPlan.ApplicationUserId);
            return View(appUserPlan);
        }

        // POST: AppUserPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,Price,IfLock,ApplicationUserId")] AppUserPlan appUserPlan)
        {
            if (id != appUserPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUserPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserPlanExists(appUserPlan.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true), "Id", "Email", appUserPlan.ApplicationUserId);
            return View(appUserPlan);
        }

        // GET: AppUserPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUserPlan = await _context.AppUserPlans
                .Include(a => a.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appUserPlan == null)
            {
                return NotFound();
            }

            return View(appUserPlan);
        }

        // POST: AppUserPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appUserPlan = await _context.AppUserPlans.SingleOrDefaultAsync(m => m.Id == id);
            _context.AppUserPlans.Remove(appUserPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserPlanExists(int id)
        {
            return _context.AppUserPlans.Any(e => e.Id == id);
        }
        public ActionResult AddAppUserPlan (string UserId)
        {
            if (_context.AppUserPlans.Where(a => a.ApplicationUserId == UserId).Any(a => a.StartDate < DateTime.Today && a.EndDate >= DateTime.Today))
            {
                return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId, Err = "User Already have a plan right now." });
            }
            else
            {
                AppUserPlan appUserPlan = new AppUserPlan();
                appUserPlan.ApplicationUserId = UserId;
                return View(appUserPlan);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppUserPlan(AppUserPlan appUserPlan, int EndDateNumber)
        {
            int Days = EndDateNumber * 28;
            appUserPlan.EndDate = appUserPlan.StartDate.AddDays(Days);
            if (ModelState.IsValid)
            {
                
                _context.AppUserPlans.Add(appUserPlan);            
                _context.SaveChanges();
                //return RedirectToAction("CreateDetail", "WorkoutPlansDetail", new { appUserPlanId = appUserPlan.Id });
                return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId });
            }

            return View(appUserPlan);
        }

    }
}
