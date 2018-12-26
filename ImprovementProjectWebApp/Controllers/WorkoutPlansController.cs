//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using ImprovementProjectWebApp.Data;
//using ImprovementProjectWebApp.Models;
//using ImprovementProjectWebApp.Models.PartExerciseViewModels;

//namespace ImprovementProjectWebApp.Controllers
//{
//    public class WorkoutPlansController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public WorkoutPlansController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: WorkoutPlans
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.WorkoutPlan.Include(w => w.ApplicationUser).Include(w => w.Exercise);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: WorkoutPlans/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var workoutPlan = await _context.WorkoutPlan
//                .Include(w => w.ApplicationUser)
//                .Include(w => w.Exercise)
//                .SingleOrDefaultAsync(m => m.Id == id);
//            if (workoutPlan == null)
//            {
//                return NotFound();
//            }

//            return View(workoutPlan);
//        }

//        // GET: WorkoutPlans/Create
//        public IActionResult Create()
//        {
//            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "UserName");
//            //ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name");
//            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name");
//            ViewData["PartId"] = new SelectList(_context.BodyPart, "Id", "Name");
//            return View();
//        }

//        // POST: WorkoutPlans/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Create( WorkoutPlan workoutPlan)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        workoutPlan.CreatedDate = DateTime.Now;
//        //        _context.Add(workoutPlan);
//        //        //await _context.SaveChangesAsync();
//        //        return View(AddWorkoutPlanView(workoutPlan));
//        //    }
//        //    else
//        //    {
//        //        ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", workoutPlan.UserId);
//        //        ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
//        //        ViewData["PartId"] = new SelectList(_context.BodyPart, "Id", "Name", workoutPlan.Exercise.BodyPart.Id);
//        //        return View(workoutPlan);
//        //    }

//        //}


//        //==============back up code ======================


//        //// GET: WorkoutPlans/Create
//        //public IActionResult Create()
//        //{
//        //    ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "UserName");
//        //    //ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name");
//        //    ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name");
//        //    ViewData["PartId"] = new SelectList(_context.BodyPart, "Id", "Name");
//        //    return View();
//        //}

//        //// POST: WorkoutPlans/Create
//        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(WorkoutPlan workoutPlan)
//        {
//            if (ModelState.IsValid)
//            {
//                workoutPlan.CreatedDate = DateTime.Now;
//                _context.Add(workoutPlan);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", workoutPlan.UserId);
//            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
//            ViewData["PartId"] = new SelectList(_context.BodyPart, "Id", "Name", workoutPlan.Exercise.BodyPart.Id);
//            return View(workoutPlan);
//        }
//        //==================================================


//        // GET: WorkoutPlans/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var workoutPlan = await _context.WorkoutPlan.SingleOrDefaultAsync(m => m.Id == id);
//            if (workoutPlan == null)
//            {
//                return NotFound();
//            }
//            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", workoutPlan.UserId);
//            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
//            return View(workoutPlan);
//        }

//        // POST: WorkoutPlans/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, WorkoutPlan workoutPlan)
//        {
//            if (id != workoutPlan.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(workoutPlan);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!WorkoutPlanExists(workoutPlan.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", workoutPlan.UserId);
//            ViewData["ExerciseId"] = new SelectList(_context.Exercise, "Id", "Name", workoutPlan.ExerciseId);
//            return View(workoutPlan);
//        }

//        // GET: WorkoutPlans/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var workoutPlan = await _context.WorkoutPlan
//                .Include(w => w.ApplicationUser)
//                .Include(w => w.Exercise)
//                .SingleOrDefaultAsync(m => m.Id == id);
//            if (workoutPlan == null)
//            {
//                return NotFound();
//            }

//            return View(workoutPlan);
//        }

//        // POST: WorkoutPlans/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var workoutPlan = await _context.WorkoutPlan.SingleOrDefaultAsync(m => m.Id == id);
//            _context.WorkoutPlan.Remove(workoutPlan);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool WorkoutPlanExists(int id)
//        {
//            return _context.WorkoutPlan.Any(e => e.Id == id);
//        }


//        public ActionResult ExerciseSeletor()
//        {
//            PartExercise partExercise = new PartExercise();
//            var PExercise = _context.BodyPart;
//            var Exercieses = _context.Exercise;
//            partExercise.BodyParts = PExercise.ToList();
//            partExercise.Exercises = Exercieses.ToList();
//            return View(partExercise);
//        }

//        public ActionResult SelectPart(string selectedPart)
//        {
//            var ListBodyPart = from x in _context.BodyPart select x;
//            var ListExercieses = from x in _context.Exercise where x.BodyPart.Name == selectedPart select x;
//            PartExercise partExercise = new PartExercise();
//            partExercise.Exercises = ListExercieses.ToList();
//            partExercise.BodyParts = ListBodyPart.ToList();
//            return PartialView("_ExerciseSelectorPV", partExercise);
//            //return Json(ListExercieses);
//        }
//        public ActionResult ListWorkoutPlans(string planName)
//        {
//            List<WorkoutPlan> workoutPlans = new List<WorkoutPlan>();
//            workoutPlans = (from x in _context.WorkoutPlan where x.PlanName == planName select x).Include(x => x.ApplicationUser).Include(x => x.Exercise).ToList();
//            //foreach (var item in workoutPlans)
//            //{
//            //    item.ApplicationUser = (from x in _context.WorkoutPlan where x == item select x.ApplicationUser).FirstOrDefault();
//            //    item.Exercise = (from x in _context.WorkoutPlan where x == item select x.Exercise).FirstOrDefault();
//            //}
//            return PartialView("_ListWorkoutPlanPV", workoutPlans);

//        }

//        public JsonResult SelectExercise(string selectedExercise)
//        {
//            var Exercieses = from x in _context.Exercise where x.Name == selectedExercise select x;
//            int ExerciseId = Exercieses.FirstOrDefault().Id;
//            return Json(ExerciseId);
//        }

//        //[HttpPost]
//        //public ActionResult CreateEx(WorkoutEx ex)
//        //{
//        //    return View();
//        //}


//        public IActionResult AddWorkoutPlanView(string PlanName, string UserId, int PartId, int ExId, string err)
//        {
//            if (PlanName != null)
//            {
//                PlanName = PlanName.ToLower();

//                PartExercise partExercise = new PartExercise();
//                WorkoutExercise workoutExercise = new WorkoutExercise();
//                partExercise.UserId = UserId;
//                partExercise.PlanName = PlanName;
//                var PExercise = _context.BodyPart;
//                partExercise.BodyParts = PExercise.ToList();
//                if (_context.Exercise.Where(e => e.Id == ExId).FirstOrDefault() != null)
//                {
//                    PartId = _context.Exercise.Where(e => e.Id == ExId).FirstOrDefault().BodyPartId;
//                }
//                var Exercieses = _context.Exercise.Where(e => e.BodyPartId == PartId);
//                partExercise.Exercises = Exercieses.ToList();

//                partExercise.SelectExerciseId = ExId;
//                partExercise.SelectPartId = PartId;

//                List<PlanSetsReps> planSetsReps = new List<PlanSetsReps>();

//                if (_context.WorkoutPlan.Where(w => w.UserId == UserId).Count() == 0 || _context.WorkoutPlan.Where(w => w.UserId == UserId).Where(w => w.PlanTrackId >= 0).Count() == 0)
//                {
//                    partExercise.PlanTrackNumber = 0;
//                }
//                else
//                {
//                    var PlanWithPlanName = _context.WorkoutPlan.Where(w => w.UserId == UserId).Where(w => w.PlanName == PlanName);
//                    if (PlanWithPlanName.Count() == 0)
//                    {
//                        var NewPlanTrackId = _context.WorkoutPlan.Where(w => w.UserId == UserId).Max(w => w.PlanTrackId) + 1;
//                        partExercise.PlanTrackNumber = NewPlanTrackId;
//                    }
//                    else
//                    {
//                        var PlanTrackId = (from x in _context.WorkoutPlan where x.PlanName == PlanName select x).FirstOrDefault().PlanTrackId;
//                        partExercise.PlanTrackNumber = PlanTrackId;
//                    }
//                }





//                if (_context.WorkoutPlan.Where(w => w.PlanName == PlanName).Where(w => w.UserId == UserId).Count() != 0)
//                {
//                    List<WorkoutPlan> workoutPlans = (_context.WorkoutPlan.Where(w => w.PlanName == PlanName).Where(w => w.ApplicationUser.Id == UserId).Include(w => w.ApplicationUser).Include(w => w.Exercise)).ToList();
//                    foreach (var planReps in workoutPlans)
//                    {
//                        PlanSetsReps eachPlanSetReps = new PlanSetsReps();
//                        eachPlanSetReps.WorkoutPlan = planReps;
//                        eachPlanSetReps.Reps = (_context.Reps.Where(r => r.WorkoutPlanId == planReps.Id)).ToList();
//                        planSetsReps.Add(eachPlanSetReps);
//                    }
//                    workoutExercise.ListPlanSetsReps = planSetsReps;

//                }
//                else
//                {
//                    WorkoutPlan wPlan = new WorkoutPlan();
//                    wPlan.UserId = UserId;
//                    wPlan.PlanName = PlanName;
//                    workoutExercise.WorkoutPlan = wPlan;
//                }




//                if (_context.Exercise.Where(e => e.Id == ExId).FirstOrDefault() != null)
//                {
//                    workoutExercise.CreateBTNShow = true;
//                }
//                partExercise.Err = err;
//                workoutExercise.PartExercise = partExercise;


//                return View(workoutExercise);
//            }
//            else
//            {
//                return RedirectToAction("ViewPlan", new { id = UserId });
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult AddWorkoutPlan(WorkoutExercise workoutExercise)
//        {

//            WorkoutPlan workoutPlan = workoutExercise.WorkoutPlan;
//            if (workoutPlan.OtherTypeExercise == true || (workoutPlan.OtherTypeExercise == false && workoutPlan.Sets != 0))
//            {


//                if (workoutPlan.OtherTypeExercise == true)
//                {
//                    workoutPlan.Sets = 0;
//                    if (workoutPlan.Des == null)
//                    {
//                        int partId = _context.Exercise.Where(e => e.Id == workoutPlan.ExerciseId).FirstOrDefault().BodyPartId;
//                        int exId = workoutPlan.ExerciseId;
//                        string err = "Please enter some description for this workout.";
//                        return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId, exId, partId, err });
//                    }
//                }
//                workoutPlan.CreatedDate = DateTime.Now;
//                if (workoutExercise.ProgressiveOverload == true)
//                {
//                    _context.WorkoutPlan.Add(workoutPlan);
//                    int workoutSets = workoutPlan.Sets;
//                    int workoutPlanId = workoutPlan.Id;
//                    int POreps = 20;
//                    if (workoutSets > 0)
//                    {
//                        for (int i = 0; i < workoutSets; i++)
//                        {
//                            Reps reps = new Reps();
//                            reps.num = POreps;
//                            reps.WorkoutPlanId = workoutPlanId;
//                            POreps = POreps - 2;
//                            _context.Reps.Add(reps);

//                        }
//                        _context.SaveChanges();
//                        return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId });
//                    }

//                }
//                _context.WorkoutPlan.Add(workoutPlan);

//                _context.SaveChanges();
//                //RedirectToAction("AddWorkoutPlanView",new { PlanName = workoutExercise.WorkoutPlan.PlanName, UserId = workoutExercise.WorkoutPlan.UserId});

//                if (workoutPlan.OtherTypeExercise == true)
//                {
//                    return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId });
//                }
//                else
//                {
//                    return RedirectToAction("RepsView", workoutPlan);
//                }
//            }
//            else
//            {
//                int partId = _context.Exercise.Where(e => e.Id == workoutPlan.ExerciseId).FirstOrDefault().BodyPartId;
//                int exId = workoutPlan.ExerciseId;
//                string err = "Set can not be empty!!!";
//                return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId, exId, partId, err });
//            }
//        }

//        public ActionResult RepsView(WorkoutPlan workoutPlan)
//        {
//            RepsPlanIdVM repsPlanIdVM = new RepsPlanIdVM();
//            repsPlanIdVM.Set = workoutPlan.Sets;
//            repsPlanIdVM.WorkoutPlanId = workoutPlan.Id;
//            repsPlanIdVM.PlanName = workoutPlan.PlanName;
//            repsPlanIdVM.UserId = workoutPlan.UserId;

//            return View("RepsView", repsPlanIdVM);
//        }
//        public ActionResult anotherRepsView(int Id)
//        {
//            WorkoutPlan workoutPlan = _context.WorkoutPlan.Where(w => w.Id == Id).FirstOrDefault();
//            RepsPlanIdVM repsPlanIdVM = new RepsPlanIdVM();
//            repsPlanIdVM.Set = workoutPlan.Sets;
//            repsPlanIdVM.WorkoutPlanId = workoutPlan.Id;
//            repsPlanIdVM.PlanName = workoutPlan.PlanName;
//            repsPlanIdVM.UserId = workoutPlan.UserId;

//            return View("RepsView", repsPlanIdVM);
//            //return Json(ListExercieses);
//        }
//        public ActionResult AddReps(string[] repsArray, string workPlanId)
//        {
//            int workoutPlanId = int.Parse(workPlanId);
//            var repsExist = _context.Reps.Where(r => r.WorkoutPlanId == workoutPlanId);
//            if (repsExist.Count() != 0)
//            {
//                _context.Reps.RemoveRange(repsExist);

//            }

//            foreach (var item in repsArray)
//            {
//                Reps reps = new Reps();
//                reps.num = int.Parse(item);
//                reps.WorkoutPlanId = workoutPlanId;
//                _context.Reps.Add(reps);
//            }
//            _context.SaveChanges();
//            string planName = _context.WorkoutPlan.Where(x => x.Id == workoutPlanId).FirstOrDefault().PlanName;
//            string userId = _context.WorkoutPlan.Where(x => x.Id == workoutPlanId).FirstOrDefault().UserId;
//            return Json("success");

//        }

//        public async Task<IActionResult> SearchCustomer(string searchString)
//        {
//            var customers = from c in _context.ApplicationUser
//                            select c;

//            if (!String.IsNullOrEmpty(searchString))
//            {
//                customers = customers.Where(c => c.UserName.Contains(searchString));

//            }

//            return View(await customers.ToListAsync());
//        }
//        public ActionResult ViewCustomer(string id)
//        {
//            var customers = from c in _context.ApplicationUser
//                            select c;

//            if (!String.IsNullOrEmpty(id))
//            {
//                ApplicationUser customer = (customers.Where(c => c.Id == id)).FirstOrDefault();
//                return View(customer);
//            }
//            return View();
//        }

//        public ActionResult ViewPlan(string id)
//        {
//            var customers = from c in _context.ApplicationUser
//                            select c;
//            if (customers.Count() > 0)
//            {
//                ApplicationUser customer = (customers.Where(c => c.Id == id)).FirstOrDefault();
//                UserPlanVM userPlanVM = new UserPlanVM();
//                userPlanVM.ApplicationUser = customer;
//                //List<WorkoutPlan> workoutPlans = new List<WorkoutPlan>();
//                var customerPlans = from x in _context.WorkoutPlan where x.ApplicationUser == customer group x by x.PlanTrackId;
//                var planNames = from x in customerPlans select x.FirstOrDefault().PlanName;
//                userPlanVM.PlanName = planNames.ToList();
//                userPlanVM.count = planNames.Count();
//                return View(userPlanVM);
//            }
//            return View();

//        }

//        public ActionResult WorkoutEditView(int Id, int PartId, int ExId, string Err)//Id = workoutPlanId
//        {
//            WorkoutExercise workoutEx = new WorkoutExercise();
//            PartExercise partExercise = new PartExercise();
//            workoutEx.WorkoutPlan = _context.WorkoutPlan.Where(w => w.Id == Id).FirstOrDefault();

//            if (_context.BodyPart.Where(b => b.Id == PartId).Count() == 0 && _context.Exercise.Where(e => e.Id == ExId).Count() == 0)
//            {

//                var PExercise = _context.BodyPart;
//                int exerciseId = workoutEx.WorkoutPlan.ExerciseId;
//                int PId = _context.Exercise.Where(e => e.Id == exerciseId).FirstOrDefault().BodyPartId;
//                var Exercieses = _context.Exercise.Where(e => e.BodyPartId == PId);
//                partExercise.BodyParts = PExercise.ToList();
//                partExercise.Exercises = Exercieses.ToList();
//                partExercise.SelectPartId = PId;
//                partExercise.SelectExerciseId = exerciseId;
//                workoutEx.CreateBTNShow = true;
//            }
//            else if (_context.BodyPart.Where(b => b.Id == PartId).Count() != 0 && _context.Exercise.Where(e => e.Id == ExId).Count() == 0)
//            {
//                var PExercise = _context.BodyPart;
//                var Exercieses = _context.Exercise.Where(e => e.BodyPartId == PartId);
//                partExercise.BodyParts = PExercise.ToList();
//                partExercise.Exercises = Exercieses.ToList();
//                partExercise.SelectPartId = PartId;
//            }
//            else if (_context.BodyPart.Where(b => b.Id == PartId).Count() == 0 && _context.Exercise.Where(e => e.Id == ExId).Count() != 0)
//            {
//                var PExercise = _context.BodyPart;
//                int PId = _context.Exercise.Where(e => e.Id == ExId).FirstOrDefault().BodyPartId;
//                var Exercieses = _context.Exercise.Where(e => e.BodyPartId == PId);
//                partExercise.BodyParts = PExercise.ToList();
//                partExercise.Exercises = Exercieses.ToList();
//                partExercise.SelectPartId = PId;
//                partExercise.SelectExerciseId = ExId;
//                workoutEx.WorkoutPlan.ExerciseId = ExId;
//                workoutEx.CreateBTNShow = true;
//            }
//            else if (_context.BodyPart.Where(b => b.Id == PartId).Count() != 0 && _context.Exercise.Where(e => e.Id == ExId).Count() != 0)
//            {
//                var PExercise = _context.BodyPart;
//                var Exercieses = _context.Exercise.Where(e => e.BodyPartId == PartId);
//                partExercise.BodyParts = PExercise.ToList();
//                partExercise.Exercises = Exercieses.ToList();
//                partExercise.SelectPartId = PartId;
//                partExercise.SelectExerciseId = ExId;
//                workoutEx.WorkoutPlan.ExerciseId = ExId;
//                workoutEx.CreateBTNShow = true;
//            }

//            partExercise.PlanID = Id;
//            partExercise.Err = Err;
//            workoutEx.PartExercise = partExercise;

//            return View(workoutEx);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> WorkoutEdit(WorkoutExercise workoutExercise)
//        {

//            WorkoutPlan workoutPlan = workoutExercise.WorkoutPlan;
//            if (workoutPlan.OtherTypeExercise == true || (workoutPlan.OtherTypeExercise == false && workoutPlan.Sets != 0))
//            {
//                if (workoutPlan.OtherTypeExercise == true)
//                {
//                    if (workoutPlan.Des == null)
//                    {
//                        int partId = _context.Exercise.Where(e => e.Id == workoutPlan.ExerciseId).FirstOrDefault().BodyPartId;
//                        int exId = workoutPlan.ExerciseId;
//                        string err = "Please enter description!!!";
//                        return RedirectToAction("WorkoutEditView", new { workoutPlan.Id, partId, exId, err });
//                    }
//                    else
//                    {
//                        _context.Update(workoutPlan);
//                        _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == workoutPlan.Id));
//                        _context.SaveChanges();

//                        return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId });
//                    }
//                }


//                if (workoutExercise.ProgressiveOverload == true)
//                {
//                    //update workoutPlan first
//                    _context.Update(workoutPlan);
//                    _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == workoutPlan.Id));
//                    //empty reps

//                    //add new reps in the workout Plan 
//                    int workoutSets = workoutPlan.Sets;
//                    int workoutPlanId = workoutPlan.Id;
//                    int POreps = 20;
//                    if (workoutSets > 0)
//                    {
//                        for (int i = 0; i < workoutSets; i++)
//                        {
//                            Reps reps = new Reps();
//                            reps.num = POreps;
//                            reps.WorkoutPlanId = workoutPlanId;
//                            POreps = POreps - 2;
//                            _context.Reps.Add(reps);

//                        }
//                        _context.SaveChanges();
//                        return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId });
//                    }
//                    else
//                    {
//                        int partId = _context.Exercise.Where(e => e.Id == workoutPlan.ExerciseId).FirstOrDefault().BodyPartId;
//                        int exId = workoutPlan.ExerciseId;
//                        string err = "Set can not be 0!!!";
//                        return RedirectToAction("WorkoutEditView", new { workoutPlan.Id, partId, exId, err });
//                    }

//                }

//                _context.Update(workoutPlan);
//                _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == workoutPlan.Id));
//                await _context.SaveChangesAsync();
//                if (workoutPlan.OtherTypeExercise == true)
//                {
//                    return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId });
//                }
//                else
//                {
//                    return RedirectToAction("RepsView", workoutPlan);
//                }
//            }
//            else
//            {
//                int partId = _context.Exercise.Where(e => e.Id == workoutPlan.ExerciseId).FirstOrDefault().BodyPartId;
//                int exId = workoutPlan.ExerciseId;
//                string err = "Set can not be empty!!!";
//                return RedirectToAction("WorkoutEditView", new { workoutPlan.Id, partId, exId, err });
//            }

//        }



//        public async Task<IActionResult> WorkoutPlanDelete(int id)
//        {
//            var workoutPlan = await _context.WorkoutPlan.SingleOrDefaultAsync(m => m.Id == id);
//            _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == id));
//            _context.WorkoutPlan.Remove(workoutPlan);
//            await _context.SaveChangesAsync();
//            return RedirectToAction("AddWorkoutPlanView", new { workoutPlan.PlanName, workoutPlan.UserId });
//        }

//    }
//}
