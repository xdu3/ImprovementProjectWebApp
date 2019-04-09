using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImprovementProjectWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var order = await _db.AppUserPlans.Include(a => a.ApplicationUser).Include(a => a.PlanPackage).FirstOrDefaultAsync(a => a.Id == id);

            return View(order);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<AppUserPlan> userPlans = await _db.AppUserPlans
                                                   .Include(o => o.ApplicationUser)
                                                   .Include(a => a.PlanPackage)
                                                   .Where(u => u.ApplicationUserId == claim.Value)
                                                   .ToListAsync();

            return View(userPlans);
        }

        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int Id)
        {
            AppUserPlan userPlan = await _db.AppUserPlans
                                            .Include(a => a.ApplicationUser)
                                            .Include(a => a.PlanPackage)
                                            .FirstOrDefaultAsync(a => a.Id == Id);

            return PartialView("_IndividualOrderDetails", userPlan);

        }
    }
}