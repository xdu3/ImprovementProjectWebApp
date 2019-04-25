using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.AccountViewModels;
//using ImprovementProjectWebApp.Models.Customer;
using ImprovementProjectWebApp.Models.DashboardVM;
using ImprovementProjectWebApp.Models.PartExerciseViewModels;
using ImprovementProjectWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger,
            ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }



        public IActionResult Index()
        {
            DIndexVM dIndexVM = new DIndexVM();
            //==================Total User======================================
            dIndexVM.CurUser = _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Count();
            //===============User without plan======================================
            List<ApplicationUser> UserWithPlan = _context.AppUserPlans.Select(a => a.ApplicationUser).ToList();
            dIndexVM.UserWithoutPlan = _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Except(UserWithPlan).Count();
            //=====================User plan Expired====================================
            List<ApplicationUser> applicationUsers = _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Include(c => c.CustomerProfiles).Include(c => c.AppUserPlans).Where(u => u.AppUserPlans.Count() != 0).ToList();
            List<ApplicationUser> finalUser = new List<ApplicationUser>();
            foreach (var item in applicationUsers)
            {
                if ((item.AppUserPlans.Max(a => a.EndDate) <= DateTime.Today))
                {
                    finalUser.Add(item);
                }
            }
            dIndexVM.UserExpired = finalUser.Count();
            //==============User about to expired=========================
            dIndexVM.UserAboutToExpired = _context.AppUserPlans.Where(a => (a.EndDate-DateTime.Today).Days <= 5 && (a.EndDate - DateTime.Today).Days >= 0).Count();
            //==================User Have Plan 
            //==========================
            dIndexVM.MealPlanRequired = _context.WeekPlan.Where(w => w.WeekPlanStartTime <= DateTime.Today.AddDays(3) && w.WeekPlanStartTime >= DateTime.Today).Where(w => w.Plans.Count() == 7).Where(w => w.MealPlan.FirstOrDefault() == null).Select(w => w.AppUserPlan.ApplicationUser).Count();
            //MealPlanRequried==============
            dIndexVM.WorkoutPlanRequried = _context.WeekPlan.Where(w => w.WeekPlanStartTime <= DateTime.Today.AddDays(3) && w.WeekPlanStartTime >= DateTime.Today).Where(w => w.Plans.Count() != 7).Where(w => w.MealPlan.FirstOrDefault() != null).Select(w => w.AppUserPlan.ApplicationUser).Count();
            //======workoutPlan required======================

            dIndexVM.CustomerNeedToContact = _context.ApplicationUser.Where(a => a.IntroQAs.Count() == _context.IntroQuestion.Where(i => i.IfHide == false).Count()).Where(a => a.AppUserPlans.Count() == 0).Count();
            //=================Customer Need To Contact===============
            //dIndexVM.ContactThem = _context.ApplicationUser.Where(a => a.IntroQAs.Count() == _context.IntroQuestion.Where(i => i.IfHide == false).Count()).Where(a => a.AppUserPlans.Count() == 0).Count()
            //=================Qustions===============
            dIndexVM.Qustions = _context.FeedBack.Where(f => f.Answer == null).Count();
            return View(dIndexVM);
        }
        public ActionResult MemberList(int MemberCondition)
        {
            MemberListVM memberListVM = new MemberListVM();
            var AppUsers = _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Include(c => c.CustomerProfiles).Include(c=>c.AppUserPlans).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
            List<ApplicationUser> UserWithoutPlan = AppUsers.Where(u => u.AppUserPlans.Count() == 0).ToList();
            switch (MemberCondition)
            {

                case 0:

                    memberListVM.ApplicationUsers = AppUsers;
                    break;
                case 1:

                    memberListVM.ApplicationUsers = UserWithoutPlan;
                    break;
                case 2:
                    
                    List<ApplicationUser> finalUser = new List<ApplicationUser>();
                    foreach (var item in UserWithoutPlan)
                    {
                        if (item.AppUserPlans.Count() != 0)
                        {
                            if ((item.AppUserPlans.Max(a => a.EndDate) <= DateTime.Today))
                            {
                                finalUser.Add(item);
                            }
                        }
                    }
                    memberListVM.ApplicationUsers = finalUser;
                    break;
                case 3:
                    List<ApplicationUser> UserAboutToExpired = new List<ApplicationUser>();
                    memberListVM.ApplicationUsers = _context.AppUserPlans.Include(a=>a.ApplicationUser).Where(a => (a.EndDate - DateTime.Today).Days <= 5 && (a.EndDate - DateTime.Today).Days >= 0).Select(a=>a.ApplicationUser).ToList();
                    break;
                case 4:
                    List<ApplicationUser> UserWithoutMealPlan = new List<ApplicationUser>();
                    memberListVM.ApplicationUsers = _context.WeekPlan.Where(w => w.WeekPlanStartTime <= DateTime.Today.AddDays(7) && w.WeekPlanStartTime >= DateTime.Today).Where(w => w.Plans.Count() == 7).Where(w => w.MealPlan.FirstOrDefault() == null).Select(w => w.AppUserPlan.ApplicationUser).Include(c => c.CustomerProfiles).Include(c => c.AppUserPlans).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                    break;
                case 5:
                    List<ApplicationUser> UserWithoutWorkoutPlan = new List<ApplicationUser>();
                    memberListVM.ApplicationUsers = _context.WeekPlan.Where(w => w.WeekPlanStartTime <= DateTime.Today.AddDays(7) && w.WeekPlanStartTime >= DateTime.Today).Where(w => w.Plans.Count() != 7).Where(w => w.MealPlan.FirstOrDefault() != null).Select(w => w.AppUserPlan.ApplicationUser).Include(c => c.CustomerProfiles).Include(c => c.AppUserPlans).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                    break;
                case 6:
                    List<ApplicationUser> CustomerNeedToContact = new List<ApplicationUser>();
                    memberListVM.ApplicationUsers =
                        _context.ApplicationUser.Where(a => a.IntroQAs.Count() == _context.IntroQuestion.Where(i => i.IfHide == false).Count()).Where(a=>a.AppUserPlans.Count()==0).Include(c => c.AppUserPlans).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return View(memberListVM);
        }


        [HttpGet]
        public IActionResult AddAdmin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost, ActionName("AddAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var result2 = await _userManager.AddToRoleAsync(user, "Admin");
                    //====================================================


                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                }
                AddErrors(result);
            }
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public ActionResult SearchCustomer(string searchString)
        {
            //var customers = from c in _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Include(a=>a.CustomerProfiles) select c ;
            var customers = _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Include(a => a.CustomerProfiles).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate);
            if (!String.IsNullOrEmpty(searchString))
            {
               var customers2 = _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true).Include(a => a.CustomerProfiles).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).Where(cstm => cstm.UserName.Contains(searchString) || cstm.Email.Contains(searchString) || cstm.CustomerProfiles.FirstOrDefault().Name.Contains(searchString));
                return View( customers2.ToList());
            }

            return View( customers.ToList());
        }
        public ActionResult ViewCustomer(string id)
        {
            var customers = from c in _context.ApplicationUser.Where(a => a.IfDelete == false && a.EmailConfirmed == true)
                            select c;

            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser customer = (customers.Where(c => c.Id == id)).FirstOrDefault();
                return View(customer);
            }
            return View();
        }

        public ActionResult ViewPlan(string id, string Err,int WeekPlanId)
        {
            
            DViewPlanVM viewPlanVM = new DViewPlanVM();
            AppUserPlan appUserPlan = _context.AppUserPlans.Include(a => a.ApplicationUser).Include(a=>a.WeekPlans).Where(a => a.ApplicationUserId == id).Where(a=>a.StartDate<=DateTime.Today.AddDays(7) && a.EndDate>=DateTime.Today).FirstOrDefault();
            
            if(_context.ApplicationUser.Where(a=>a.Id == id).FirstOrDefault().UserName == "dx3081@gmail.com")
            {
                viewPlanVM.IfTemplate = true;
            }
            ViewData["Err"] = Err;
            ViewData["UserName"] = _context.ApplicationUser.Where(a => a.Id == id).FirstOrDefault().UserName;
            ViewData["UserId"] = id;
            viewPlanVM.AppUserPlan = appUserPlan;
            int FirstWeekPlanId = 0;
            if (appUserPlan != null)
            {
                //==================put this here because appuserplan == null====================================
               
                //==========================
                TimeSpan span = appUserPlan.EndDate.Subtract(appUserPlan.StartDate);
                int days =  (int)span.TotalDays;
                int weeks = days / 7;

                viewPlanVM.WeekPlans = _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).ToList();
                viewPlanVM.WeekLeft = weeks - _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).Count();
                if(WeekPlanId ==0)
                {
                    if (_context.WeekPlan.Any(w => w.AppUserPlanId == appUserPlan.Id))
                    {
                        WeekPlan weekPlan = _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).FirstOrDefault();
                        viewPlanVM.SelectWeekPlansId = weekPlan.Id;
                        FirstWeekPlanId = weekPlan.Id;
                        List<Plan> Plans = _context.Plans.Where(p => p.WeekPlanId == weekPlan.Id).ToList();
                        foreach (var item in Plans)
                        {
                            item.WorkoutNum = _context.WorkoutPlan.Where(w => w.PlanId == item.Id).Count();
                        }
                        viewPlanVM.Plans = Plans;
                    }

                            viewPlanVM.IfLastWeekPlan = true;

                }
                else
                {
                    viewPlanVM.SelectWeekPlansId = WeekPlanId;
                    List<Plan> Plans = _context.Plans.Where(p => p.WeekPlanId == WeekPlanId).ToList();
                    foreach(var item in Plans)
                    {
                        item.WorkoutNum = _context.WorkoutPlan.Where(w => w.PlanId == item.Id).Count();
                    }
                    viewPlanVM.Plans = Plans;
                    if (_context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).Count() != 0)
                    {
                        int LastWeekPlanId = appUserPlan.WeekPlans.OrderByDescending(w => w.WeekPlanEndTime).First().Id;
                        if (WeekPlanId == LastWeekPlanId)
                        {
                            viewPlanVM.IfLastWeekPlan = true;
                        }
                    }
                }
                //if (_context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).Count() != 0)
                //{
                //    int LastWeekPlanId = appUserPlan.WeekPlans.OrderByDescending(w => w.WeekPlanEndTime).First().Id;
                //    if (WeekPlanId == LastWeekPlanId)
                //    {
                //        viewPlanVM.IfLastWeekPlan = true;
                //    }
                //}
            }
            if(WeekPlanId !=0)
            {
                FirstWeekPlanId = WeekPlanId;
            }
            if (_context.MealPlan.Any(m=>m.WeekPlanId == FirstWeekPlanId))
            {
                viewPlanVM.MealPlanExist = true;
                viewPlanVM.MealPlanURL = _context.MealPlan.Where(m => m.WeekPlanId == FirstWeekPlanId).FirstOrDefault().URL;
            }
            if (appUserPlan != null)
            {
                if (_context.AppUserPlans.Any(a => a.ApplicationUserId == id))
                {
                    if (_context.UserCheckInDate.Where(u => u.AppUserPlanId == appUserPlan.Id) != null)
                    {
                        viewPlanVM.CheckInStatus = _context.UserCheckInDate.Where(u => u.AppUserPlanId == appUserPlan.Id).FirstOrDefault().CheckInDate.DayOfWeek.ToString();
                    }
                }
            }
            return View(viewPlanVM);

        }
        
        public ActionResult SetUserCheckInDate(int appUserPlanId)
        {
            SetUserCheckInVM setUserCheckInVM = new SetUserCheckInVM();
            setUserCheckInVM.AppUserPlanId = appUserPlanId;
            return View(setUserCheckInVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetUserCheckInDate(SetUserCheckInVM setUserCheckInVM)
        {
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == setUserCheckInVM.AppUserPlanId).FirstOrDefault();
            int day = setUserCheckInVM.Day;
            
            int startday = (int)appUserPlan.StartDate.DayOfWeek;//start day
            int endday = day; //end day
            int daysDiff = (7 + (endday - startday)) % 7;

            DateTime finalDay = appUserPlan.StartDate.AddDays(7 + daysDiff);
            int period =  ((TimeSpan)(appUserPlan.EndDate - appUserPlan.StartDate)).Days/7;
            if (_context.UserCheckInDate.Any(u => u.AppUserPlanId == appUserPlan.Id))
            {
                List<UserCheckInDate> userCheckInDates = _context.UserCheckInDate.Where(u => u.AppUserPlanId == appUserPlan.Id).ToList();
                _context.UserCheckInDate.RemoveRange(userCheckInDates);
                _context.SaveChanges();
                
            }


            if (appUserPlan.StartDate.DayOfWeek == DayOfWeek.Monday && day == 0)
            {
                DateTime TheDate = appUserPlan.StartDate;
                for (int i = 0; i < period; i++)
                {
                    UserCheckInDate userCheckInDate = new UserCheckInDate();
                    userCheckInDate.AppUserPlanId = appUserPlan.Id;
                    if (i == 0)
                    {
                        userCheckInDate.CheckInDate = TheDate.AddDays(6);
                        TheDate = TheDate.AddDays(6);
                    }
                    else
                    {
                        userCheckInDate.CheckInDate = TheDate.AddDays(7);
                        TheDate = TheDate.AddDays(7);
                    }
                    _context.UserCheckInDate.Add(userCheckInDate);
                    _context.SaveChanges();
                }
            }
            else
            {

            }
            return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId });
            //==================back up code================================
            //AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == setUserCheckInVM.AppUserPlanId).FirstOrDefault();
            //int day = setUserCheckInVM.Day;

            //int startday = (int)appUserPlan.StartDate.DayOfWeek;//start day
            //int endday = day; //end day
            //int daysDiff = (7 + (endday - startday)) % 7;

            //DateTime finalDay = appUserPlan.StartDate.AddDays(7 + daysDiff);
            //int period = ((TimeSpan)(appUserPlan.EndDate - appUserPlan.StartDate)).Days / 7;
            //if (!_context.UserCheckInDate.Any(u => u.AppUserPlanId == appUserPlan.Id))
            //{
            //    for (int i = 0; i < period; i++)
            //    {
            //        UserCheckInDate userCheckInDate = new UserCheckInDate();
            //        userCheckInDate.AppUserPlanId = appUserPlan.Id;
            //        userCheckInDate.CheckInDate = finalDay.AddDays(i * 7);
            //        _context.UserCheckInDate.Add(userCheckInDate);
            //        _context.SaveChanges();
            //    }
            //}
            //else
            //{
            //    List<UserCheckInDate> userCheckInDates = _context.UserCheckInDate.Where(u => u.AppUserPlanId == appUserPlan.Id).ToList();

            //}
            //return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId });

        }
        public ActionResult ViewUserCheckInQA(string UserId)
        {
            ViewData["UserId"] = UserId;
            return View(_context.CheckInQA.Where(c => c.ApplicationUserId == UserId).Include(c => c.ApplicationUser).ToList());
        }

        public ActionResult ViewUserProfile(string UserId)
        {
            CustomerProfile customerProfile = _context.CustomerProfile.Where(c => c.ApplicationUserId == UserId).Include(c => c.ApplicationUser).FirstOrDefault();
            return View(customerProfile);
        }

        public ActionResult FastEditPlan(int appUserPlanId,int PlanId, string Err)
        {
            ViewData["Err"] = Err;
            FastEditPlanVM fastEditPlanVM = new FastEditPlanVM();
            fastEditPlanVM.plans = _context.Plans.Where(p => p.WeekPlan.AppUserPlanId == appUserPlanId).Include(p => p.WeekPlan).ThenInclude(p => p.AppUserPlan).ToList();
            fastEditPlanVM.CopyPlanId = PlanId;
            fastEditPlanVM.UserId = _context.AppUserPlans.Where(a => a.Id == appUserPlanId).FirstOrDefault().ApplicationUserId;
            List<WeekPlan> WeekPlanWithPlan = _context.Plans.Where(p => p.WeekPlan.AppUserPlanId == appUserPlanId).Select(p => p.WeekPlan).ToList();
            fastEditPlanVM.WeekPlans = _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlanId).Except(WeekPlanWithPlan).ToList();
            return View(fastEditPlanVM);
        }
        public ActionResult DuplicationWeekPlan(int WeekPlanId)
        {
            
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == weekPlan.AppUserPlanId).FirstOrDefault();//
            int period = ((TimeSpan)(appUserPlan.EndDate - appUserPlan.StartDate)).Days;
            int totalWeekPlan = period / 7;
            int WeekPlanCount = _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).Count();
            if (WeekPlanCount < totalWeekPlan)
            {
                WeekPlan newWeekPlan = new WeekPlan();
                //newWeekPlan.AppUserPlanId = weekPlan.AppUserPlanId;//
                newWeekPlan.AppUserPlanId = appUserPlan.Id;
                newWeekPlan.Name = weekPlan.Name;
                //WeekPlan lastWeekPlan = _context.WeekPlan.Where(a => a.AppUserPlanId == appUserPlan.Id).Max(w=>w.WeekPlanEndTime);
                DateTime lastWeekPlanEndTime = _context.WeekPlan.Where(a => a.AppUserPlanId == appUserPlan.Id).Max(w => w.WeekPlanEndTime);
                newWeekPlan.WeekPlanStartTime = lastWeekPlanEndTime.AddDays(1);
                newWeekPlan.WeekPlanEndTime = lastWeekPlanEndTime.AddDays(8);
                _context.WeekPlan.Add(newWeekPlan);
                _context.SaveChanges();
                List<Plan> plans = _context.Plans.Where(p => p.WeekPlanId == weekPlan.Id).ToList();
                DateTime starttime = lastWeekPlanEndTime.AddDays(1);
                foreach (var item in plans)
                {
                    Plan plan = new Plan();
                    plan.Name = item.Name;
                    plan.WeekPlanId = newWeekPlan.Id;
                    plan.DayPlanNum = item.DayPlanNum;
                    plan.DayPlanDate = starttime.AddDays(item.DayPlanNum);
                    _context.Plans.Add(plan);
                    _context.SaveChanges();
                    List<WorkoutPlan> workoutPlans = _context.WorkoutPlan.Where(w => w.PlanId == item.Id).ToList();
                    foreach (var i in workoutPlans)
                    {
                        WorkoutPlan workoutPlan = new WorkoutPlan();
                        workoutPlan.Des = i.Des;
                        workoutPlan.ExerciseId = i.ExerciseId;
                        workoutPlan.OtherTypeExercise = i.OtherTypeExercise;
                        workoutPlan.PlanId = plan.Id;
                        workoutPlan.ProgressiveOverload = i.ProgressiveOverload;
                        workoutPlan.Sets = i.Sets;
                        _context.WorkoutPlan.Add(workoutPlan);
                        _context.SaveChanges();
                        List<Reps> reps = _context.Reps.Where(w => w.WorkoutPlanId == i.Id).ToList();
                        foreach (var x in reps)
                        {
                            Reps newReps = new Reps();
                            newReps.num = x.num;
                            newReps.WorkoutPlanId = workoutPlan.Id;
                            _context.Reps.Add(newReps);
                            _context.SaveChanges();

                        }
                    }
                }
                return RedirectToAction("FastEditPlan", "Dashboard", new { appUserPlanId = appUserPlan.Id });
            }
            return RedirectToAction("FastEditPlan", "Dashboard", new { appUserPlanId = appUserPlan.Id,Err="Can't add more plan." });
        }
        public ActionResult PastePlanToDate(int AppUserPlanId,  int WeekPlanId, int DayNum,int CopyPlanId)
        {
            Plan plan = _context.Plans.Where(p => p.Id == CopyPlanId).FirstOrDefault();
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();
            Plan newPlan = new Plan();
            newPlan.Name = plan.Name;
            newPlan.WeekPlanId = WeekPlanId;
            newPlan.DayPlanDate = weekPlan.WeekPlanStartTime.AddDays(DayNum);
            newPlan.DayPlanNum = DayNum;
            _context.Plans.Add(newPlan);
            _context.SaveChanges();
            List<WorkoutPlan> workoutPlans = _context.WorkoutPlan.Where(w => w.PlanId == plan.Id).ToList();
            foreach (var i in workoutPlans)
            {
                WorkoutPlan workoutPlan = new WorkoutPlan();
                workoutPlan.Des = i.Des;
                workoutPlan.ExerciseId = i.ExerciseId;
                workoutPlan.OtherTypeExercise = i.OtherTypeExercise;
                workoutPlan.PlanId = newPlan.Id;
                workoutPlan.ProgressiveOverload = i.ProgressiveOverload;
                workoutPlan.Sets = i.Sets;
                _context.WorkoutPlan.Add(workoutPlan);
                _context.SaveChanges();
                List<Reps> reps = _context.Reps.Where(w => w.WorkoutPlanId == i.Id).ToList();
                foreach (var x in reps)
                {
                    Reps newReps = new Reps();
                    newReps.num = x.num;
                    newReps.WorkoutPlanId = workoutPlan.Id;
                    _context.Reps.Add(newReps);
                    _context.SaveChanges();

                }
            }
            return RedirectToAction("FastEditPlan", "Dashboard", new { appUserPlanId = weekPlan.AppUserPlanId });

        }
        
        public ActionResult CheckInDetail(int CheckinQAId)
        {
            string UserId = _context.CheckInQA.Where(c => c.Id == CheckinQAId).Select(c => c.ApplicationUserId).FirstOrDefault();
            ViewData["UserId"] = UserId;
            CheckInDetailVM checkInDetailVM = new CheckInDetailVM();
            checkInDetailVM.IntroQA = _context.IntroQA.Where(i => i.UserId == UserId).Where(i => i.IntroQuestion.Question.Contains("当前体重（空腹）")).FirstOrDefault();
            var checkInQAs = _context.CheckInQA.Where(c => c.Id == CheckinQAId).ToList();

            if (CheckinQAId != 0)
            {
                if (_context.CheckInQADetail.Any(c => c.CheckInQAId == CheckinQAId))
                {
                    checkInDetailVM.CheckInQADetails = _context.CheckInQADetail.Where(c => c.CheckInQAId == CheckinQAId).Include(c => c.CheckInQuestion).ToList();
                }

                checkInDetailVM.IMGUrl = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckinQAId).Select(c => c.ImgURL).ToList();

            }
            return View(checkInDetailVM);
        }
        public ActionResult ViewWeekPlan(int WeekPlanId, string UserId,int AppUserPlanId)
        {
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).Include(w => w.Plans).ThenInclude(p => p.WorkoutPlans).ThenInclude(p => p.Exercise).Include(w => w.Plans).ThenInclude(p => p.WorkoutPlans).ThenInclude(w => w.Reps).FirstOrDefault();
            ViewWeekPlanVM viewWeekPlanVM = new ViewWeekPlanVM();
            viewWeekPlanVM.WeekPlan = weekPlan;
            viewWeekPlanVM.WeekPlanId = WeekPlanId;
            viewWeekPlanVM.UserId = UserId;
            viewWeekPlanVM.AppUserPlanId = AppUserPlanId;
            return View(viewWeekPlanVM);
        }
        public ActionResult AddTemplate( int AppUserPlanId,string UserId)
        {
            AddTemplateVM addTemplateVM = new AddTemplateVM();
            addTemplateVM.weekPlans = _context.WeekPlan.Where(w => w.AppUserPlan.ApplicationUser.UserName == "dx3081@gmail.com").ToList();
            addTemplateVM.AppUserPlanId = AppUserPlanId;
            addTemplateVM.UserId = UserId;
            addTemplateVM.ApplicationUser = _context.ApplicationUser.Where(a => a.Id == UserId).FirstOrDefault();
            return View(addTemplateVM);
        }

        public ActionResult CopyTemplate(int WeekPlanId,int AppUserPlanId)
        {

            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault();//
            int period = ((TimeSpan)(appUserPlan.EndDate - appUserPlan.StartDate)).Days;
            int totalWeekPlan = period / 7;
            int WeekPlanCount = _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlan.Id).Count();
            if (WeekPlanCount < totalWeekPlan)
            {
                WeekPlan newWeekPlan = new WeekPlan();
                //newWeekPlan.AppUserPlanId = weekPlan.AppUserPlanId;//
                newWeekPlan.AppUserPlanId = appUserPlan.Id;
                newWeekPlan.Name = weekPlan.Name;
                //WeekPlan lastWeekPlan = _context.WeekPlan.Where(a => a.AppUserPlanId == appUserPlan.Id).Max(w=>w.WeekPlanEndTime);
                DateTime lastWeekPlanEndTime;
                if (_context.WeekPlan.Where(a => a.AppUserPlanId == appUserPlan.Id).Count() != 0)
                {
                    lastWeekPlanEndTime = _context.WeekPlan.Where(a => a.AppUserPlanId == appUserPlan.Id).Max(w => w.WeekPlanEndTime);
                    newWeekPlan.WeekPlanStartTime = lastWeekPlanEndTime.AddDays(1);
                    newWeekPlan.WeekPlanEndTime = lastWeekPlanEndTime.AddDays(7);
                }
                else
                {
                    lastWeekPlanEndTime = appUserPlan.StartDate;
                    newWeekPlan.WeekPlanStartTime = appUserPlan.StartDate;
                    newWeekPlan.WeekPlanEndTime = appUserPlan.StartDate.AddDays(6);
                }
                
                _context.WeekPlan.Add(newWeekPlan);
                _context.SaveChanges();
                List<Plan> plans = _context.Plans.Where(p => p.WeekPlanId == weekPlan.Id).ToList();
                DateTime starttime = lastWeekPlanEndTime.AddDays(1);
                foreach (var item in plans)
                {
                    Plan plan = new Plan();
                    plan.Name = item.Name;
                    plan.WeekPlanId = newWeekPlan.Id;
                    plan.DayPlanNum = item.DayPlanNum;
                    plan.DayPlanDate = starttime.AddDays(item.DayPlanNum);
                    _context.Plans.Add(plan);
                    _context.SaveChanges();
                    List<WorkoutPlan> workoutPlans = _context.WorkoutPlan.Where(w => w.PlanId == item.Id).ToList();
                    foreach (var i in workoutPlans)
                    {
                        WorkoutPlan workoutPlan = new WorkoutPlan();
                        workoutPlan.Des = i.Des;
                        workoutPlan.ExerciseId = i.ExerciseId;
                        workoutPlan.OtherTypeExercise = i.OtherTypeExercise;
                        workoutPlan.PlanId = plan.Id;
                        workoutPlan.ProgressiveOverload = i.ProgressiveOverload;
                        workoutPlan.Sets = i.Sets;
                        _context.WorkoutPlan.Add(workoutPlan);
                        _context.SaveChanges();
                        List<Reps> reps = _context.Reps.Where(w => w.WorkoutPlanId == i.Id).ToList();
                        foreach (var x in reps)
                        {
                            Reps newReps = new Reps();
                            newReps.num = x.num;
                            newReps.WorkoutPlanId = workoutPlan.Id;
                            _context.Reps.Add(newReps);
                            _context.SaveChanges();

                        }
                    }
                }
                return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId, WeekPlanId = newWeekPlan.Id });
            }
            return RedirectToAction("ViewPlan", "Dashboard", new { appUserPlanId = appUserPlan.Id, Err = "Can't add more plan." });
        }
        public ActionResult ViewCheckInDate(string UserId)
        {

            ViewCheckInDateVM viewCheckInDateVM = new ViewCheckInDateVM();
            viewCheckInDateVM.userCheckInDates = _context.UserCheckInDate.Where(u => u.AppUserPlan.ApplicationUserId == UserId).ToList();
            return View(viewCheckInDateVM);

        }
        public ActionResult ViewCheckIn(string UserId)
        {
            ViewCheckInVM viewCheckInVM = new ViewCheckInVM();
            viewCheckInVM.ApplicationUser = _context.ApplicationUser.Include(a=>a.CheckInQAs).ThenInclude(c=>c.CheckInImgs).Include(a => a.CheckInQAs).ThenInclude(c => c.CheckInQADetails).ThenInclude(d=>d.CheckInQuestion).Where(a => a.Id == UserId).FirstOrDefault();
            return View(viewCheckInVM);
        }
        public ActionResult DeleteUserCheckInDatail( int CheckInQAId,string UserId)
        {
            CheckInQA checkInQA = _context.CheckInQA.Where(c => c.Id == CheckInQAId).FirstOrDefault();
            List<CheckInQADetail> checkInQADetail = _context.CheckInQADetail.Where(c => c.CheckInQAId == checkInQA.Id).ToList();
            List<CheckInImgs> checkInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == checkInQA.Id).ToList();
            foreach (var item in checkInImgs)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
                var path = webRootPath + item.ImgURL;

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.CheckInImgs.RemoveRange(checkInImgs);
            _context.CheckInQADetail.RemoveRange(checkInQADetail);
            _context.CheckInQA.Remove(checkInQA);
            _context.SaveChanges();
            return RedirectToAction("ViewCheckIn", "Dashboard", new { UserId });
        }
        public ActionResult EditCheckInQADetail(int CheckInQADetailId)
        {
            CheckInQADetail checkInQADetail = _context.CheckInQADetail.Include(c=>c.CheckInQuestion).Where(c => c.Id == CheckInQADetailId).FirstOrDefault();
            return View(checkInQADetail);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCheckInQADetail(CheckInQADetail checkInQADetail)
        {
            string UserId = _context.CheckInQADetail.Where(c => c.Id == checkInQADetail.Id).Select(c => c.CheckInQA.ApplicationUserId).FirstOrDefault();
            _context.CheckInQADetail.Update(checkInQADetail);
            _context.SaveChanges();
            return RedirectToAction("ViewCheckIn", "Dashboard", new { UserId });
        }
        public ActionResult AddTodayAsCheckInDate(int AppUserPlanId)
        {
            AddTodayAsCheckInDateVM addTodayAsCheckInDateVM = new AddTodayAsCheckInDateVM();
            addTodayAsCheckInDateVM.UserId = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault().ApplicationUserId;
            if (_context.UserCheckInDate.Where(u => u.AppUserPlanId == AppUserPlanId).Any(u => u.CheckInDate == DateTime.Today))
            {
                addTodayAsCheckInDateVM.AlreadyExist = true;
            }
            else
            {
               
                UserCheckInDate userCheckInDate = new UserCheckInDate();
                userCheckInDate.AppUserPlanId = AppUserPlanId;
                userCheckInDate.CheckInDate = DateTime.Today;
                _context.UserCheckInDate.Add(userCheckInDate);
                _context.SaveChanges();
                addTodayAsCheckInDateVM.AddSuccess = true;

            }
            return View(addTodayAsCheckInDateVM);
        }
        public ActionResult DailyCheckIn(int daysBefore)
        {
            DailyCheckInVM dailyCheckInVM = new DailyCheckInVM();
            DateTime TargetDate = DateTime.Today;
            if (daysBefore == 0)
            {
                dailyCheckInVM.Date = TargetDate;
            }
            else
            {
                dailyCheckInVM.Date = TargetDate.AddDays(daysBefore);
                TargetDate = TargetDate.AddDays(daysBefore);
            }
            //List<ApplicationUser> applicationUsers = _context.UserCheckInDate.Where(u => u.CheckInDate == TargetDate).Select(u => u.AppUserPlan.ApplicationUser).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInQADetails).ThenInclude(u => u.CheckInQuestion).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInImgs).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.MealPlan).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.Plans).Include(u => u.CustomerProfiles).ToList();
            List<ApplicationUser> applicationUsers = _context.CheckInQA.Where(c => c.CreatedDate == TargetDate)
                .Select(u => u.ApplicationUser)
                .Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInQADetails).ThenInclude(u => u.CheckInQuestion).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInImgs)
                .Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.MealPlan).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.Plans)
                .Include(u => u.CustomerProfiles).ToList();
            dailyCheckInVM.ApplicationUsers = applicationUsers;
            return View(dailyCheckInVM);
        }
        public ActionResult WeekLyCheckIn()
        {
            DailyCheckInVM dailyCheckInVM = new DailyCheckInVM();
            DateTime TargetDate = DateTime.Today;
            
            //List<ApplicationUser> applicationUsers = _context.UserCheckInDate.Where(u => u.CheckInDate == TargetDate).Select(u => u.AppUserPlan.ApplicationUser).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInQADetails).ThenInclude(u => u.CheckInQuestion).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInImgs).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.MealPlan).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.Plans).Include(u => u.CustomerProfiles).ToList();
            List<ApplicationUser> applicationUsers = _context.AppUserPlans.Where(a=>a.StartDate<=DateTime.Today&& a.EndDate>=DateTime.Today).Select(u => u.ApplicationUser).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInQADetails).ThenInclude(u => u.CheckInQuestion).Include(u => u.CheckInQAs).ThenInclude(u => u.CheckInImgs).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.MealPlan).Include(u => u.AppUserPlans).ThenInclude(u => u.WeekPlans).ThenInclude(u => u.Plans).Include(u => u.CustomerProfiles).ToList();
            dailyCheckInVM.ApplicationUsers = applicationUsers;
            return View(dailyCheckInVM);
        }

        public ActionResult GetAllQuestion()
        {
            GetAllQuestionVM getAllQuestionVM = new GetAllQuestionVM();
            getAllQuestionVM.FeedBacks = _context.FeedBack.Include(f=>f.ApplicationUser).ThenInclude(f=>f.CustomerProfiles).ToList();
            return View(getAllQuestionVM);
        }
        public ActionResult AnswerQuestionsForAll(int FeedbackId)
        {
            FeedBack feedBack = _context.FeedBack.Where(f => f.Id == FeedbackId).FirstOrDefault();
            return View(feedBack);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerQuestionsForAll(FeedBack feedBack)
        {
            _context.FeedBack.Update(feedBack);
            _context.SaveChanges();
            return RedirectToAction("GetAllQuestion", "Dashboard");

        }

        public ActionResult UnsolvedQuestions()
        {
            GetAllQuestionVM getAllQuestionVM = new GetAllQuestionVM();
            getAllQuestionVM.FeedBacks = _context.FeedBack.Where(f=>f.Answer == null).Include(f => f.ApplicationUser).ThenInclude(f => f.CustomerProfiles).ToList();
            return View(getAllQuestionVM);
        }
        public ActionResult AnswerQuestions(int FeedbackId)
        {
            FeedBack feedBack = _context.FeedBack.Where(f => f.Id == FeedbackId).FirstOrDefault();
            return View(feedBack);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerQuestions(FeedBack feedBack)
        {
            _context.FeedBack.Update(feedBack);
            _context.SaveChanges();
            return RedirectToAction("UnsolvedQuestions", "Dashboard");

        }
    }
}