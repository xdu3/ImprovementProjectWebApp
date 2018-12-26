using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.PlanVM;
using System.Net.Mail;
using System.Net;

namespace ImprovementProjectWebApp.Controllers
{
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plans.ToListAsync());
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanName,Active,IfTemplate")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanName,Active,IfTemplate")] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            return View(plan);
        }

        // GET: Plans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
        public ActionResult AddPlan(string PlanName, string UserId ,int AppUserPlanId)
        {
            if (!string.IsNullOrEmpty(PlanName) && !string.IsNullOrEmpty(UserId))
            {
                if (!_context.Plans.Any(p => p.PlanName == PlanName))
                {
                    PlanUser PU = new PlanUser();
                    PU.User = _context.ApplicationUser.Where(a => a.Id == UserId).FirstOrDefault();
                    PU.Plan = new Plan();
                    PU.Plan.PlanName = PlanName;
                    PU.AppUserPlanId = AppUserPlanId;
                    return View(PU);
                }
                string Err1 = "Plan name already in the system. Please user another one.";
                return RedirectToAction("ViewPlan", "Dashboard", new { Id = UserId, Err = Err1 });
            }
            string Err = "Plan name can not be empty.";
            return RedirectToAction("ViewPlan", "Dashboard", new { Id = UserId, Err = Err });
           
        }
        public async Task<IActionResult> CreatePlan(Plan Plan, string UserId,int AppUserPlanId)
        {
            if (ModelState.IsValid ||_context.Plans.Where(p=>p.PlanName == Plan.PlanName).Count()==0)
            {
                _context.Add(Plan);
                await _context.SaveChangesAsync();
                int planId = _context.Plans.Where(p => p.PlanName == Plan.PlanName).FirstOrDefault().Id;
                _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault().PlanId = planId;
                await _context.SaveChangesAsync();
                string email = _context.ApplicationUser.Where(a => a.Id == UserId).FirstOrDefault().Email;
                string send = SendEmail(email, "您有一个新的健身计划。");
                return RedirectToAction("ViewPlan", "Dashboard", new { Id = UserId});
            }
            return View(Plan);
        }
        public string SendEmail(string email, string bodyInfo)
        {
            string Feedback = "";
            if (!string.IsNullOrEmpty(email))
            {
                var result = _context.ApplicationUser.Any(e => e.Email == email);
                if (result)
                {
                    try
                    {
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);


                        client.EnableSsl = true;


                        MailAddress from = new MailAddress("dxtestemail@gmail.com", "du xin");


                        MailAddress to = new MailAddress(email, "Your recepient name");


                        MailMessage message = new MailMessage(from, to);


                        message.Body = bodyInfo;


                        message.Subject = "ER Project 新信息";


                        NetworkCredential myCreds = new NetworkCredential("dxtestemail@gmail.com", "21568328", "");


                        client.Credentials = myCreds;

                        client.Send(message);

                        Feedback = "Email have been already send";
                        return Feedback;
                    }
                    catch (Exception)
                    {
                        Feedback = "There is something wrong when you send Email.";
                        return Feedback;

                    }

                }
                else
                {
                    Feedback = "This email have been already taken";
                    return Feedback;
                }

            }
            else
            {
                Feedback = "please enter a email to register";
                return Feedback;

            }
        }
    }
}
