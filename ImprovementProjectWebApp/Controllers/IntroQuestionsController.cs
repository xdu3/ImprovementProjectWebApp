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
    public class IntroQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IntroQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IntroQuestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.IntroQuestion.Where(i=>i.IfHide ==false).ToListAsync());
        }

        // GET: IntroQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introQuestion = await _context.IntroQuestion
                .SingleOrDefaultAsync(m => m.Id == id);
            if (introQuestion == null)
            {
                return NotFound();
            }

            return View(introQuestion);
        }

        // GET: IntroQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IntroQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question")] IntroQuestion introQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(introQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(introQuestion);
        }

        // GET: IntroQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introQuestion = await _context.IntroQuestion.SingleOrDefaultAsync(m => m.Id == id);
            if (introQuestion == null)
            {
                return NotFound();
            }
            return View(introQuestion);
        }

        // POST: IntroQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question")] IntroQuestion introQuestion)
        {
            if (id != introQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(introQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntroQuestionExists(introQuestion.Id))
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
            return View(introQuestion);
        }

        // GET: IntroQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introQuestion = await _context.IntroQuestion
                .SingleOrDefaultAsync(m => m.Id == id);
            if (introQuestion == null)
            {
                return NotFound();
            }

            return View(introQuestion);
        }

        // POST: IntroQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var introQuestion = await _context.IntroQuestion.SingleOrDefaultAsync(m => m.Id == id);
            introQuestion.IfHide = true;
            _context.Update(introQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IntroQuestionExists(int id)
        {
            return _context.IntroQuestion.Any(e => e.Id == id);
        }
    }
}
