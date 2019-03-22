using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.WorkoutDatailVM;
using ImprovementProjectWebApp.Models.PartExerciseViewModels;

namespace ImprovementProjectWebApp.Controllers
{
    public class WorkoutPlansDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutPlansDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkoutPlansDetail
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WorkoutPlan.Include(w => w.Exercise).Include(w => w.Plan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WorkoutPlansDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlan
                .Include(w => w.Exercise)
                .Include(w => w.Plan)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // GET: WorkoutPlansDetail/Create
        public IActionResult Create()
        {
            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name");
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName");
            return View();
        }

        // POST: WorkoutPlansDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sets,OtherTypeExercise,Des,ExerciseId,PlanId")] WorkoutPlan workoutPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workoutPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName", workoutPlan.PlanId);
            return View(workoutPlan);
        }

        // GET: WorkoutPlansDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlan.SingleOrDefaultAsync(m => m.Id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName", workoutPlan.PlanId);
            return View(workoutPlan);
        }

        // POST: WorkoutPlansDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sets,OtherTypeExercise,Des,ExerciseId,PlanId")] WorkoutPlan workoutPlan)
        {
            if (id != workoutPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutPlanExists(workoutPlan.Id))
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
            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "PlanName", workoutPlan.PlanId);
            return View(workoutPlan);
        }

        // GET: WorkoutPlansDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlan
                .Include(w => w.Exercise)
                .Include(w => w.Plan)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // POST: WorkoutPlansDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutPlan = await _context.WorkoutPlan.SingleOrDefaultAsync(m => m.Id == id);
            _context.WorkoutPlan.Remove(workoutPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutPlanExists(int id)
        {
            return _context.WorkoutPlan.Any(e => e.Id == id);
        }


        public IActionResult CreateDetail(int PlanId, int PartId, int ExId, string Err)
        {
            ViewData["Err"] = Err;
            ExerciseSelectorVM ESVM = new ExerciseSelectorVM();

            PartExercise PE = new PartExercise();

            //WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();

            if (PartId != 0 || ExId != 0)
            {
                ESVM.IfTemplate = false;
            }
            PE.BodyParts = _context.BodyPart.ToList();
            PE.SelectPartId = PartId;
            PE.SelectExerciseId = ExId;
            PE.PlanId = PlanId;
            if (PartId > 0)
            {
                PE.Exercises = _context.Exercise.Where(e => e.BodyPartId == PartId).ToList();
            }
            else
            {
                if (ExId > 0)
                {
                    PE.SelectPartId = _context.Exercise.Where(e => e.Id == ExId).Select(e => e.BodyPartId).FirstOrDefault();
                    PE.Exercises = _context.Exercise.Where(x => x.BodyPartId == (_context.Exercise.Where(e => e.Id == ExId).Select(e => e.BodyPartId).FirstOrDefault())).ToList();
                    WorkoutPlan WP = new WorkoutPlan();
                    WP.PlanId = PlanId;
                    WP.ExerciseId = ExId;
                    ESVM.WorkoutPlan = WP;
                }
            }
            ESVM.PartExercise = PE;
            //================list workout plan PV================

            List<WorkoutPlan> wps = _context.WorkoutPlan.Include(w => w.Plan).Include(w => w.Exercise).Where(w => w.PlanId == PlanId).ToList();
            if (wps.Count != 0)
            {
                //List<Reps> reps = _context.Reps.Include(r => r.WorkoutPlan).Where(r => r.WorkoutPlan.PlanId == planId).ToList();
                List<PlanSetsReps> planSetsReps = new List<PlanSetsReps>();
                foreach (var planReps in wps)
                {
                    PlanSetsReps eachPlanSetReps = new PlanSetsReps();
                    eachPlanSetReps.WorkoutPlan = planReps;
                    eachPlanSetReps.Reps = (_context.Reps.Where(r => r.WorkoutPlanId == planReps.Id)).ToList();
                    planSetsReps.Add(eachPlanSetReps);
                }
                ESVM.ListPlanSetsReps = planSetsReps;
            }
            ESVM.UserID = _context.Plans.Where(w => w.Id == PlanId).Select(w => w.WeekPlan.AppUserPlan.ApplicationUserId).FirstOrDefault();
            ESVM.WeekPlanId = _context.Plans.Where(w => w.Id == PlanId).Select(w => w.WeekPlanId).FirstOrDefault();
            return View(ESVM);
        }

        // POST: WorkoutPlansDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDetail(WorkoutPlan workoutPlan)
        {
            int PlanId = workoutPlan.PlanId;
            int WeekPlanId = PlanId;
            if (ModelState.IsValid)
            {
                if (workoutPlan.OtherTypeExercise == false && workoutPlan.Sets == 0)
                {
                    int ExId = workoutPlan.ExerciseId;
                    string Err = "Set Can't be 0.";
                    return RedirectToAction("CreateDetail", new { PlanId, ExId, Err });

                }
                _context.Add(workoutPlan);

                await _context.SaveChangesAsync();
                if (workoutPlan.ProgressiveOverload == false && workoutPlan.OtherTypeExercise == false)
                {
                    int workoutPlanId = workoutPlan.Id;
                    return RedirectToAction("RepsView", new { workoutPlanId });
                }
                else
                {
                    _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == workoutPlan.Id));
                    //empty reps

                    //add new reps in the workout Plan 
                    int workoutSets = workoutPlan.Sets;
                    //
                    int workoutPlanId = workoutPlan.Id;
                    int POreps = 20;
                    for (int i = 0; i < workoutSets; i++)
                    {
                        Reps reps = new Reps();
                        reps.num = POreps;
                        reps.WorkoutPlanId = workoutPlanId;
                        POreps = POreps - 2;
                        _context.Reps.Add(reps);

                    }
                    _context.SaveChanges();
                    return RedirectToAction("CreateDetail", new { PlanId });
                    // there is no situation workoutSets is 0

                }
            }
            return RedirectToAction("CreateDetail", new { PlanId });
        }

        public IActionResult EditDetail(int WorkoutPlanId, int PartId, int ExId, string Err)
        {

            ViewData["Err"] = Err;
            ExerciseSelectorVM ESVM = new ExerciseSelectorVM();
            PartExercise PE = new PartExercise();
            PE.WorkoutPlanId = WorkoutPlanId;
            //AppUserPlan AUP = _context.AppUserPlans.Where(a => a.Id == appUserPlanId).FirstOrDefault();
            PE.BodyParts = _context.BodyPart.ToList();

            PE.SelectPartId = PartId;
            PE.SelectExerciseId = ExId;
            ESVM.WorkoutPlan = _context.WorkoutPlan.Where(w => w.Id == WorkoutPlanId).FirstOrDefault();
            if (PartId == 0 && ExId == 0)
            {
                ExId = _context.WorkoutPlan.Where(w => w.Id == WorkoutPlanId).FirstOrDefault().ExerciseId;
            }
            //PE.AppUserPlanId = appUserPlanId;
            if (PartId != 0)
            {
                PE.Exercises = _context.Exercise.Where(e => e.BodyPartId == PartId).ToList();
            }
            else
            {
                if (ExId != 0)
                {
                    PE.SelectPartId = _context.Exercise.Where(e => e.Id == ExId).Select(e => e.BodyPartId).FirstOrDefault();
                    PE.Exercises = _context.Exercise.Where(x => x.BodyPartId == (_context.Exercise.Where(e => e.Id == ExId).Select(e => e.BodyPartId).FirstOrDefault())).ToList();
                    //WP.PlanId = AUP.PlanId;
                    PE.SelectExerciseId = ExId;
                    ESVM.WorkoutPlan.ExerciseId = ExId;
                }
            }
            ESVM.PartExercise = PE;
            ESVM.UserID = _context.WorkoutPlan.Where(w => w.Id == WorkoutPlanId).Select(w => w.Plan.WeekPlan.AppUserPlan.ApplicationUserId).FirstOrDefault();
            ESVM.WeekPlanId = _context.WorkoutPlan.Where(w => w.Id == WorkoutPlanId).Select(w => w.Plan.WeekPlanId).FirstOrDefault();
            return View(ESVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetail(WorkoutPlan workoutPlan)
        {
            int PlanId = workoutPlan.PlanId;
            if (ModelState.IsValid)
            {
                if (workoutPlan.OtherTypeExercise == false && workoutPlan.Sets == 0)
                {
                    int ExId = workoutPlan.ExerciseId;
                    string Err = "Set Can't be 0.";
                    return RedirectToAction("EditDetail", new { WorkoutPlanId = workoutPlan.Id, ExId, Err });

                }
                _context.Update(workoutPlan);

                await _context.SaveChangesAsync();
                if (workoutPlan.ProgressiveOverload == false)
                {
                    int workoutPlanId = workoutPlan.Id;
                    return RedirectToAction("RepsView", new { workoutPlanId = workoutPlan.Id });
                }
                else
                {
                    _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == workoutPlan.Id));
                    //empty reps

                    //add new reps in the workout Plan 
                    int workoutSets = workoutPlan.Sets;
                    //
                    int workoutPlanId = workoutPlan.Id;
                    int POreps = 20;
                    for (int i = 0; i < workoutSets; i++)
                    {
                        Reps reps = new Reps();
                        reps.num = POreps;
                        reps.WorkoutPlanId = workoutPlanId;
                        POreps = POreps - 2;
                        _context.Reps.Add(reps);

                    }
                    _context.SaveChanges();
                    return RedirectToAction("CreateDetail", new { PlanId });
                    // there is no situation workoutSets is 0

                }
            }
            return RedirectToAction("CreateDetail", new { PlanId });
        }




        public ActionResult RepsView(int workoutPlanId)
        {
            WorkoutPlan workoutPlan = _context.WorkoutPlan.Where(w => w.Id == workoutPlanId).FirstOrDefault();
            RepsPlanIdVM repsPlanIdVM = new RepsPlanIdVM();
            repsPlanIdVM.Set = workoutPlan.Sets;
            repsPlanIdVM.WorkoutPlanId = workoutPlan.Id;

            return View("RepsView", repsPlanIdVM);
        }
        public ActionResult AddReps(string[] repsArray, string workPlanId)
        {
            int workoutPlanId = int.Parse(workPlanId);
            var repsExist = _context.Reps.Where(r => r.WorkoutPlanId == workoutPlanId);
            if (repsExist.Count() != 0)
            {
                _context.Reps.RemoveRange(repsExist);

            }

            foreach (var item in repsArray)
            {
                Reps reps = new Reps();
                reps.num = int.Parse(item);
                reps.WorkoutPlanId = workoutPlanId;
                _context.Reps.Add(reps);
            }
            _context.SaveChanges();
            int PlanId = _context.WorkoutPlan.Select(w => w.PlanId).FirstOrDefault();
            //string planName = _context.WorkoutPlan.Where(x => x.Id == workoutPlanId).FirstOrDefault().PlanName;
            //string userId = _context.WorkoutPlan.Where(x => x.Id == workoutPlanId).FirstOrDefault().UserId;
            return Json("PlanId");
        }

        public ActionResult FinishDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //List<Reps> repsAndWorkoutPlan = _context.Reps.Include(r => r.WorkoutPlan).ThenInclude(w => w.Plan).Include(r => r.WorkoutPlan).ThenInclude(w => w.Exercise).Where(r => r.WorkoutPlanId == id).ToList();
            //int PlanId = repsAndWorkoutPlan.FirstOrDefault().WorkoutPlan.PlanId;
            //ViewData["PlanId"] = PlanId;
            //if (repsAndWorkoutPlan == null)
            //{
            //    return NotFound();
            //}

            //return View(repsAndWorkoutPlan);
            int PlanId = _context.WorkoutPlan.Where(w => w.Id == id).Select(w=>w.PlanId).FirstOrDefault();
            return RedirectToAction("CreateDetail", new { PlanId });
        }

        public async Task<IActionResult> WorkoutPlanDelete(int id)
        {
            var workoutPlan = await _context.WorkoutPlan.SingleOrDefaultAsync(m => m.Id == id);
            _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == id));
            int PlanId = _context.WorkoutPlan.Where(w => w.Id == id).FirstOrDefault().PlanId;

            _context.WorkoutPlan.Remove(workoutPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction("CreateDetail", new { PlanId });
        }
    }
}
