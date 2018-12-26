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
    public class CheckInQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckInQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CheckInQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.CheckInQuestion.Where(c=>c.IfHide==false).ToListAsync());
        }

        // GET: CheckInQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkInQuestion = await _context.CheckInQuestion
                .SingleOrDefaultAsync(m => m.Id == id);
            if (checkInQuestion == null)
            {
                return NotFound();
            }

            return View(checkInQuestion);
        }

        // GET: CheckInQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CheckInQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question")] CheckInQuestion checkInQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checkInQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checkInQuestion);
        }

        // GET: CheckInQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkInQuestion = await _context.CheckInQuestion.SingleOrDefaultAsync(m => m.Id == id);
            if (checkInQuestion == null)
            {
                return NotFound();
            }
            return View(checkInQuestion);
        }

        // POST: CheckInQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question")] CheckInQuestion checkInQuestion)
        {
            if (id != checkInQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkInQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckInQuestionExists(checkInQuestion.Id))
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
            return View(checkInQuestion);
        }

        // GET: CheckInQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkInQuestion = await _context.CheckInQuestion
                .SingleOrDefaultAsync(m => m.Id == id);
            if (checkInQuestion == null)
            {
                return NotFound();
            }

            return View(checkInQuestion);
        }

        // POST: CheckInQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkInQuestion = await _context.CheckInQuestion.SingleOrDefaultAsync(m => m.Id == id);
            checkInQuestion.IfHide = true;
            _context.Update(checkInQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckInQuestionExists(int id)
        {
            return _context.CheckInQuestion.Any(e => e.Id == id);
        }
    }
}
