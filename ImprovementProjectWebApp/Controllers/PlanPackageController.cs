using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Controllers
{
    public class PlanPackageController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PlanPackageController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.PlanPackage.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanPackage plan)
        {
            if (ModelState.IsValid)
            {
                _db.PlanPackage.Add(plan);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _db.PlanPackage.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _db.PlanPackage.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlanPackage plan)
        {
            if (ModelState.IsValid)
            {
                _db.Update(plan);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _db.PlanPackage.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _db.PlanPackage.FindAsync(id);
            if (plan == null)
            {
                return View();
            }

            _db.PlanPackage.Remove(plan);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}