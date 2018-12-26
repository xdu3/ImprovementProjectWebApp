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
    public class BodyPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BodyPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BodyParts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BodyPart.ToListAsync());
        }

        // GET: BodyParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyPart = await _context.BodyPart
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bodyPart == null)
            {
                return NotFound();
            }

            return View(bodyPart);
        }

        // GET: BodyParts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BodyParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] BodyPart bodyPart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bodyPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bodyPart);
        }

        // GET: BodyParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyPart = await _context.BodyPart.SingleOrDefaultAsync(m => m.Id == id);
            if (bodyPart == null)
            {
                return NotFound();
            }
            return View(bodyPart);
        }

        // POST: BodyParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] BodyPart bodyPart)
        {
            if (id != bodyPart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bodyPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyPartExists(bodyPart.Id))
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
            return View(bodyPart);
        }

        // GET: BodyParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyPart = await _context.BodyPart
                .SingleOrDefaultAsync(m => m.Id == id);
            if (bodyPart == null)
            {
                return NotFound();
            }

            return View(bodyPart);
        }

        // POST: BodyParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bodyPart = await _context.BodyPart.SingleOrDefaultAsync(m => m.Id == id);
            _context.BodyPart.Remove(bodyPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyPartExists(int id)
        {
            return _context.BodyPart.Any(e => e.Id == id);
        }
    }
}
