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
            var applicationDbContext = _context.AppUserPlans.Include(a => a.ApplicationUser).Include(a => a.Plan);
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
                .Include(a => a.Plan)
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName");
            return View();
        }

        // POST: AppUserPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanName,StartDate,EndDate,Price,IfLock,PlanId,ApplicationUserId")] AppUserPlan appUserPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUserPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", appUserPlan.ApplicationUserId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName", appUserPlan.PlanId);
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", appUserPlan.ApplicationUserId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName", appUserPlan.PlanId);
            return View(appUserPlan);
        }

        // POST: AppUserPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanName,StartDate,EndDate,Price,IfLock,PlanId,ApplicationUserId")] AppUserPlan appUserPlan)
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", appUserPlan.ApplicationUserId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName", appUserPlan.PlanId);
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
                .Include(a => a.Plan)
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



        public IActionResult CreateRelationship(string UserID)
        {
            if (_context.AppUserPlans.Where(a => a.ApplicationUserId == UserID).Any(a => a.StartDate < DateTime.Today && a.EndDate >= DateTime.Today))
            {
                return RedirectToAction("ViewPlan", "Dashboard", new { id = UserID, Err = "User Already have a plan right now." });
            }
            else
            {
                

                AppUserPlan appUserPlan = new AppUserPlan();
                appUserPlan.ApplicationUserId = UserID;
                return View(appUserPlan);
            }
            
        }

        // POST: AppUserPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRelationship( AppUserPlan appUserPlan,int EndDateNumber)
        {
            if (ModelState.IsValid)
            {
                int trackid = 1;
                if(_context.AppUserPlans.Count() !=0)
                {
                    trackid = _context.AppUserPlans.Max(a => a.TrackId) + 1;
                }

                for (int i = 0; i < EndDateNumber; i++)
                {
                    if(_context.Plans.Where(p=>p.PlanName == "未完成").Count()==0)
                    {
                        Plan plan = new Plan();
                        plan.PlanName = "未完成";
                        _context.Plans.Add(plan);
                        _context.SaveChanges();
                        
                    }
                    if(_context.MealPlan.Where(m=>m.Default == true).Count() == 0)
                    {
                        MealPlan mealPlan = new MealPlan();
                        mealPlan.Default = true;
                        _context.MealPlan.Add(mealPlan);
                        _context.SaveChanges();
                    }
                    

                    if (i == 0)
                    {
                        AppUserPlan NewAUP = new AppUserPlan();
                        NewAUP.Price = appUserPlan.Price;
                        NewAUP.IfLock = appUserPlan.IfLock;
                        NewAUP.ApplicationUserId = appUserPlan.ApplicationUserId;
                        NewAUP.StartDate = DateTime.Today;
                        NewAUP.EndDate = DateTime.Today.AddDays(28);
                        NewAUP.TrackId = trackid;
                        NewAUP.PlanId = _context.Plans.Where(p => p.PlanName == "未完成").FirstOrDefault().Id;
                        NewAUP.MealPlanId = _context.MealPlan.Where(m => m.Default == true).FirstOrDefault().Id;
                        _context.AppUserPlans.Add(NewAUP);
                    }
                    else
                    {
                        AppUserPlan NewAUP = new AppUserPlan();
                        NewAUP.Price = appUserPlan.Price;
                        NewAUP.IfLock = appUserPlan.IfLock;
                        NewAUP.ApplicationUserId = appUserPlan.ApplicationUserId;
                        NewAUP.StartDate = DateTime.Today.AddDays(i*28+1);
                        NewAUP.EndDate = DateTime.Today.AddDays((i+1)*28);
                        NewAUP.TrackId = trackid;
                        NewAUP.PlanId = _context.Plans.Where(p => p.PlanName == "未完成").FirstOrDefault().Id;
                        NewAUP.MealPlanId = _context.MealPlan.Where(m => m.Default == true).FirstOrDefault().Id;
                        _context.AppUserPlans.Add(NewAUP);
                    }
                   
                    
                }
                _context.SaveChanges();
                //return RedirectToAction("CreateDetail", "WorkoutPlansDetail", new { appUserPlanId = appUserPlan.Id });
                return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId});
            }
            
            return View(appUserPlan);
        }
    }
}
