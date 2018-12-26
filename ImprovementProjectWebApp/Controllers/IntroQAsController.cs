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
    public class IntroQAsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IntroQAsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IntroQAs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IntroQA.Include(i => i.ApplicationUser).Include(i => i.IntroQuestion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IntroQAs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introQA = await _context.IntroQA
                .Include(i => i.ApplicationUser)
                .Include(i => i.IntroQuestion)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (introQA == null)
            {
                return NotFound();
            }

            return View(introQA);
        }

        // GET: IntroQAs/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            ViewData["IntroQuestionId"] = new SelectList(_context.IntroQuestion, "Id", "Question");
            return View();
        }

        // POST: IntroQAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Answer,CreatedDate,UserId,IntroQuestionId")] IntroQA introQA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(introQA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", introQA.UserId);
            ViewData["IntroQuestionId"] = new SelectList(_context.IntroQuestion, "Id", "Question", introQA.IntroQuestionId);
            return View(introQA);
        }

        // GET: IntroQAs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introQA = await _context.IntroQA.SingleOrDefaultAsync(m => m.Id == id);
            if (introQA == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", introQA.UserId);
            ViewData["IntroQuestionId"] = new SelectList(_context.IntroQuestion, "Id", "Question", introQA.IntroQuestionId);
            return View(introQA);
        }

        // POST: IntroQAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Answer,CreatedDate,UserId,IntroQuestionId")] IntroQA introQA)
        {
            if (id != introQA.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(introQA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntroQAExists(introQA.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", introQA.UserId);
            ViewData["IntroQuestionId"] = new SelectList(_context.IntroQuestion, "Id", "Question", introQA.IntroQuestionId);
            return View(introQA);
        }

        // GET: IntroQAs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var introQA = await _context.IntroQA
                .Include(i => i.ApplicationUser)
                .Include(i => i.IntroQuestion)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (introQA == null)
            {
                return NotFound();
            }

            return View(introQA);
        }

        // POST: IntroQAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var introQA = await _context.IntroQA.SingleOrDefaultAsync(m => m.Id == id);
            _context.IntroQA.Remove(introQA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IntroQAExists(int id)
        {
            return _context.IntroQA.Any(e => e.Id == id);
        }
        //===============================================================
        public ActionResult IntroAnswerView(string id)
        {
            List<IntroQuestion> ListIntroQuestions = _context.IntroQuestion.ToList();
            List<IntroQA> ListIntroQAs = new List<IntroQA>();
            foreach(var item in ListIntroQuestions)
            {
                IntroQA introQA = new IntroQA();
                introQA.IntroQuestionId = item.Id;
                introQA.UserId = id;
                introQA.CreatedDate = DateTime.Now;
                introQA.IntroQuestion = item;
                ListIntroQAs.Add(introQA);
            }           
            return View(ListIntroQAs);
        }

        public ActionResult AdminIntroAnswerView(string id)
        {
            ApplicationUser appUser = _context.ApplicationUser.Where(a => a.Id == id).FirstOrDefault();
            List<IntroQuestion> ListIntroQuestions = _context.IntroQuestion.ToList();
            List<IntroQA> ListIntroQAs = new List<IntroQA>();
            List<IntroQA> UserIntroQAs = _context.IntroQA.Where(i => i.UserId == id).ToList();
            ViewData["UserName"] = appUser.UserName;
            return View(UserIntroQAs);
        }
    }
}
