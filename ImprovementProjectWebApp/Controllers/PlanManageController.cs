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
using ImprovementProjectWebApp.Models.PlanManageVM;
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
    public class PlanManageController : Controller
    {
        //private readonly ApplicationDbContext _context;

        //public PlanManageController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PlanManageController(
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
            return View();
        }
        public ActionResult AddAppUserPlan(string UserId)
        {
            if (_context.AppUserPlans.Where(a => a.ApplicationUserId == UserId).Any(a => a.StartDate < DateTime.Today && a.EndDate >= DateTime.Today))
            {
                return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId, Err = "User Already have a plan right now." });
            }
            else
            {
                AppUserPlan appUserPlan = new AppUserPlan();
                appUserPlan.ApplicationUserId = UserId;
                return View(appUserPlan);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppUserPlan(AppUserPlan appUserPlan, int EndDateNumber)
        {
            int Days = EndDateNumber * 28;
            appUserPlan.EndDate = appUserPlan.StartDate.AddDays(Days);
            if (ModelState.IsValid)
            {

                _context.AppUserPlans.Add(appUserPlan);
                _context.SaveChanges();
                if(appUserPlan.StartDate.DayOfWeek == DayOfWeek.Monday)
                {
                    int period = ((TimeSpan)(appUserPlan.EndDate - appUserPlan.StartDate)).Days / 7;
                    DateTime TheDate = appUserPlan.StartDate;
                    for (int i = 0; i < period; i++)
                    {
                        UserCheckInDate userCheckInDate = new UserCheckInDate();
                        userCheckInDate.AppUserPlanId = appUserPlan.Id;
                        if(i ==0 )
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
                return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId });
            }

            return View(appUserPlan);
        }
        public ActionResult AddWeekPlan(int appUserPlanId, string WeekPlanName)
        {
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == appUserPlanId).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(WeekPlanName))
            {

                List<WeekPlan> weekPlans = _context.WeekPlan.Where(w => w.AppUserPlanId == appUserPlanId).ToList();
                if (weekPlans.Count == 0)
                {
                    WeekPlan weekPlan = new WeekPlan();
                    weekPlan.WeekPlanStartTime = appUserPlan.StartDate;
                    weekPlan.WeekPlanEndTime = appUserPlan.StartDate.AddDays(6);
                    weekPlan.AppUserPlanId = appUserPlanId;
                    weekPlan.Name = WeekPlanName;
                    _context.WeekPlan.Add(weekPlan);
                    _context.SaveChanges();
                }
                else
                {

                    WeekPlan weekPlan = new WeekPlan();
                    weekPlan.WeekPlanStartTime = weekPlans.Max(w=>w.WeekPlanEndTime).AddDays(1);
                    weekPlan.WeekPlanEndTime = weekPlans.Max(w => w.WeekPlanEndTime).AddDays(7);
                    weekPlan.AppUserPlanId = appUserPlanId;
                    weekPlan.Name = WeekPlanName;
                    _context.WeekPlan.Add(weekPlan);
                    _context.SaveChanges();
                }

                return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId });
            }
            return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId, Err = "name can't be empty!!!" });
        }
        public ActionResult AddRestDayPlan(int WeekPlanId, int AddDayNum)
        {
            Plan plan = new Plan();
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();
            plan.DayPlanDate = weekPlan.WeekPlanStartTime.AddDays(AddDayNum);
            plan.WeekPlanId = WeekPlanId;
            plan.DayPlanNum = AddDayNum;
            plan.Name = "休息日";

            int AppUserPlanId = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault().AppUserPlanId;
            string ApplicationUserId = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault().ApplicationUserId;
            _context.Plans.Add(plan);
            _context.SaveChanges();
            bool weekPlanFinished = WeekPlanFinished(WeekPlanId);
            if (weekPlanFinished)
            {
                string UserEmail = _context.ApplicationUser.Where(a => a.Id == ApplicationUserId).Select(a => a.Email).FirstOrDefault();
                List<UserCheckInDate> userCheckInDates = _context.UserCheckInDate.Where(u => u.AppUserPlanId == AppUserPlanId).Where(u => u.CheckInDate >= DateTime.Today).ToList();
                string CheckInDate = "您接下来的打卡时间是：";
                foreach(var item in userCheckInDates)
                {
                    CheckInDate = CheckInDate + "<br/>" + item.CheckInDate.ToString("yyyy-MM-dd") + "<br/>";
                }
                CheckInDate = CheckInDate + "请您在当天24小时内打卡。如果过期无法打开哦~" + "<br/>" + "打卡请在空腹状态。" + "<br/>" + "所有食物皆为熟食重量，除燕麦" + "<br/>" + "请您避免糖分摄入" + "<br/>" + "食物烹饪可用煮，烤，或用少许油（只要不粘锅）炒。";
                string Infor = "AR计划提醒您"+"<br/>" + "您的周计划已经完成，请您登陆检查." +"<br/>"+ CheckInDate;
                _emailSender.SendEmailAsync(UserEmail, "AR Project 周计划提醒", Infor);
            }

            return RedirectToAction("ViewPlan", "Dashboard", new { id = ApplicationUserId,WeekPlanId });
        }



        public ActionResult AddDayPlan(int WeekPlanId, int AddDayNum, string Err)
        {
            Plan plan = new Plan();
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();
            plan.DayPlanDate = weekPlan.WeekPlanStartTime.AddDays(AddDayNum);
            plan.WeekPlanId = WeekPlanId;
            plan.DayPlanNum = AddDayNum;
            ViewData["Err"] = Err;
            return View(plan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDayPlan(Plan plan)
        {
            int weekPlanId = _context.WeekPlan.Where(w => w.Id == plan.WeekPlanId).FirstOrDefault().Id;
            int AppUserPlanId = _context.WeekPlan.Where(w => w.Id == weekPlanId).FirstOrDefault().AppUserPlanId;
            string ApplicationUserId = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault().ApplicationUserId;
            if (!string.IsNullOrWhiteSpace(plan.Name))
            {
                if(plan.Name !="休息日")
                {
                    if (_context.Plans.Where(p => p.WeekPlanId == weekPlanId).Where(p => p.DayPlanNum == plan.DayPlanNum).Count() == 0)
                    {
                        _context.Plans.Add(plan);
                        _context.SaveChanges();
                        bool weekPlanFinished = WeekPlanFinished(weekPlanId);
                        if (weekPlanFinished)
                        {
                            string UserEmail = _context.ApplicationUser.Where(a => a.Id == ApplicationUserId).Select(a => a.Email).FirstOrDefault();
                            List<UserCheckInDate> userCheckInDates = _context.UserCheckInDate.Where(u => u.AppUserPlanId == AppUserPlanId).Where(u => u.CheckInDate >= DateTime.Today).ToList();
                            string CheckInDate = "您接下来的打卡时间是：";
                            foreach (var item in userCheckInDates)
                            {
                                CheckInDate = CheckInDate + "<br/>" + item.CheckInDate.ToString("yyyy-MM-dd") + "<br/>";
                            }
                            CheckInDate = CheckInDate + "请您在当天24小时内打卡。如果过期无法打开哦~" + "<br/>" + "打卡请在空腹状态。" + "<br/>" + "所有食物皆为熟食重量，除燕麦" + "<br/>" + "请您避免糖分摄入" + "<br/>" + "食物烹饪可用煮，烤，或用少许油（只要不粘锅）炒。";
                            string Infor = "AR计划提醒您" + "<br/>" + "您的周计划已经完成，请您登陆检查." + "<br/>" + CheckInDate;
                            _emailSender.SendEmailAsync(UserEmail, "AR Project 周计划提醒", Infor);
                        }
                        return RedirectToAction("CreateDetail", "WorkoutPlansDetail", new { PlanId = plan.Id });
                    }
                    else
                    {
                        return RedirectToAction("ViewPlan", "Dashboard", new { id = ApplicationUserId });
                    }
                }
                return RedirectToAction("AddDayPlan", "PlanManage", new { WeekPlanId = weekPlanId, AddDayNum = plan.DayPlanNum, Err = "Plan can not be rest day!!!" });
            }
            return RedirectToAction("AddDayPlan", "PlanManage", new { WeekPlanId = weekPlanId, AddDayNum = plan.DayPlanNum, Err = "Plan Name can't be empty!!!" });
        }




        public ActionResult EditDayPlan(int PlanId)
        {
            Plan plan = _context.Plans.Where(p => p.Id == PlanId).FirstOrDefault();
            string ApplicationUserId = _context.Plans.Select(p => p.WeekPlan.AppUserPlan.ApplicationUserId).FirstOrDefault();
            ViewData["ApplicationUserId"] = ApplicationUserId;
            return View(plan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDayPlan(Plan plan)
        {
            _context.Plans.Update(plan);
            _context.SaveChanges();
            string ApplicationUserId = _context.Plans.Where(p=>p.Id == plan.Id).Select(p => p.WeekPlan.AppUserPlan.ApplicationUserId).FirstOrDefault();
            return RedirectToAction("ViewPlan", "Dashboard", new { id = ApplicationUserId });
        }
        public ActionResult DeleteDayPlan(int PlanId)
        {
            Plan plan = _context.Plans.Where(p => p.Id == PlanId).FirstOrDefault();
            int WeekPlanId = plan.WeekPlanId;
            string ApplicationUserId = _context.Plans.Where(p => p.Id == PlanId).Select(p => p.WeekPlan.AppUserPlan.ApplicationUserId).FirstOrDefault();
            DeleteDayPlanById(PlanId);
            return RedirectToAction("ViewPlan", "Dashboard", new { id = ApplicationUserId ,  WeekPlanId });
        }
        public ActionResult DeleteWeekPlan(int WeekPlanId)
        {
            string ApplicationUserId = _context.WeekPlan.Where(w => w.Id == WeekPlanId).Select(w => w.AppUserPlan.ApplicationUserId).FirstOrDefault();
            DeleteWeekPlanById(WeekPlanId);
            return RedirectToAction("ViewPlan", "Dashboard", new { id = ApplicationUserId });
        }
        public void DeleteWeekPlanById(int WeekPlanId)
        {

            List<Plan> plans = _context.Plans.Where(p => p.WeekPlanId == WeekPlanId).ToList();
            foreach (var item in plans)
            {
                DeleteDayPlanById(item.Id);
            }
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault();
            _context.Remove(weekPlan);
            _context.SaveChanges();
        }

        public void DeleteDayPlanById(int PlanId)
        {
            Plan plan = _context.Plans.Where(p => p.Id == PlanId).FirstOrDefault();
            List<WorkoutPlan> workoutPlans = _context.WorkoutPlan.Where(w => w.PlanId == PlanId).ToList();
            foreach(var item in workoutPlans)
            {
                List<Reps> reps = _context.Reps.Where(r => r.WorkoutPlanId == item.Id).ToList();
                _context.Reps.RemoveRange(reps);
            }
            _context.WorkoutPlan.RemoveRange(workoutPlans);
            _context.Plans.Remove(plan);
            _context.SaveChanges();
        }
        public ActionResult AddMealPlan(int WeekPlanId)
        {
            MealPlan mealPlan = new MealPlan();
            mealPlan.WeekPlanId = WeekPlanId;
            ViewData["WeekPlanName"] = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault().Name;
            return View(mealPlan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMealPlan(MealPlan mealPlan)
        {
            string userID = _context.WeekPlan.Where(w => w.Id == mealPlan.WeekPlanId).Select(w => w.AppUserPlan.ApplicationUserId).FirstOrDefault();
            int WeekPlanId = _context.WeekPlan.Where(w => w.Id == mealPlan.WeekPlanId).Select(w => w.Id).FirstOrDefault();
            int AppUserPlanId = _context.WeekPlan.Where(w => w.Id == WeekPlanId).FirstOrDefault().AppUserPlanId;

            var files = HttpContext.Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                string userPath = Path.Combine(newPath, userID);
                string MealPlan = "MealPlan";
                string userPath1 = Path.Combine(userPath, MealPlan);
                string subFinder = Path.Combine(userPath1, mealPlan.WeekPlanId + "");
                if (!Directory.Exists(subFinder))
                {
                    Directory.CreateDirectory(subFinder);
                }
                if (files.Count != 1)
                {
                    ModelState.AddModelError("", "You can only upload 1 file");
                    //return RedirectToAction("ViewPlan", "Dashboard", new { id = CurUserPlan.ApplicationUserId,Err = "" });
                }

                IFormFile item = files[0];
                if (item.Length > 0)
                {
                    if (item.Length > 10 * 1024 * 1024)
                    //3mb = 3*1024
                    {
                        ModelState.AddModelError("", "You can't upload file more then 10MB");
                        //return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                    }


                    var extension = item.FileName.Substring(item.FileName.LastIndexOf("."), item.FileName.Length - item.FileName.LastIndexOf("."));
                    extension = extension.ToLower();
                    string newName = _context.WeekPlan.Where(w => w.Id == WeekPlanId).Select(w => w.Name).FirstOrDefault();
                    string[] fileType = { ".pdf" };
                    if (!fileType.Contains(extension))
                    {
                        ModelState.AddModelError("", "You can only upload PDF files");                        
                    }

                    //string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(subFinder, newName + extension);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    //using (var filestream = new FileStream(fullPath, FileMode.Create))
                    //{
                    //    await item.CopyToAsync(filestream);
                    //}

                    mealPlan.URL = @"\Upload\" + userID + @"\MealPlan\" + mealPlan.WeekPlanId + @"\" + newName + extension;

                    _context.MealPlan.Add(mealPlan);
                    _context.SaveChanges();
                    bool weekPlanFinished = WeekPlanFinished(WeekPlanId);
                    if(weekPlanFinished)
                    {
                        string UserEmail = _context.ApplicationUser.Where(a => a.Id == userID).Select(a => a.Email).FirstOrDefault();
                        List<UserCheckInDate> userCheckInDates = _context.UserCheckInDate.Where(u => u.AppUserPlanId == AppUserPlanId).Where(u => u.CheckInDate >= DateTime.Today).ToList();
                        string CheckInDate = "您接下来的打卡时间是：";
                        foreach (var ixx in userCheckInDates)
                        {
                            CheckInDate = CheckInDate + "<br/>" + ixx.CheckInDate.ToString("yyyy-MM-dd") + "<br/>";
                        }
                        CheckInDate = CheckInDate + "请您在当天24小时内打卡。如果过期无法打开哦~" + "<br/>" + "打卡请在空腹状态。" + "<br/>" + "所有食物皆为熟食重量，除燕麦" + "<br/>" + "请您避免糖分摄入" + "<br/>" + "食物烹饪可用煮，烤，或用少许油（只要不粘锅）炒。";
                        string Infor = "AR计划提醒您" + "<br/>" + "您的周计划已经完成，请您登陆检查." + "<br/>" + CheckInDate;
                        _emailSender.SendEmailAsync(UserEmail, "AR Project 周计划提醒", Infor);
                    }
                    //string send = SendEmail(CurUserPlan.ApplicationUser.Email, "您有一个新的饮食计划。");
                }

            }
            return RedirectToAction("ViewPlan", "Dashboard", new { id = userID, WeekPlanId });
        }
        public ActionResult ViewWeekPlan(int WeekPlanId,string UserId)
        {
            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).Include(w => w.Plans).ThenInclude(p => p.WorkoutPlans).ThenInclude(p=>p.Exercise).Include(w => w.Plans).ThenInclude(p => p.WorkoutPlans).ThenInclude(w=>w.Reps).FirstOrDefault();
            ViewWeekPlanVM viewWeekPlanVM = new ViewWeekPlanVM();
            viewWeekPlanVM.WeekPlan = weekPlan;
            viewWeekPlanVM.WeekPlanId = WeekPlanId;
            viewWeekPlanVM.UserId = UserId;
            return View(viewWeekPlanVM);
        }
        public ActionResult DeleteMealPlan(int WeekPlanId)
        {
            string UserId = _context.WeekPlan.Where(w => w.Id == WeekPlanId).Select(w => w.AppUserPlan.ApplicationUserId).FirstOrDefault();
            bool x = DeleteMealPlanById(WeekPlanId);
            if(x == false)
            {
                return NotFound();
            }
            return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId });
        }
        public bool DeleteMealPlanById(int WeekPlanId)
        {
            MealPlan mealPlan = _context.MealPlan.Where(m => m.WeekPlanId == WeekPlanId).FirstOrDefault();
            
            if (mealPlan != null)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;
                var path = webRootPath + mealPlan.URL;

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.MealPlan.Remove(mealPlan);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool WeekPlanFinished(int WeekPlanId)
        {
            bool HaveMealPlan = false;
            bool Have7DayPlan = false;

            WeekPlan weekPlan = _context.WeekPlan.Where(w => w.Id == WeekPlanId).Include(w => w.Plans).Include(w=>w.MealPlan).FirstOrDefault();
            if(weekPlan.MealPlan.Count()!=0)
            {
                if (weekPlan.MealPlan.FirstOrDefault().URL != null)
                {
                    HaveMealPlan = true;
                }
            }
            if(weekPlan.Plans.Count()>=7)
            {
                if(weekPlan.Plans.Any(p=>p.DayPlanNum==1)&& weekPlan.Plans.Any(p => p.DayPlanNum == 2) && weekPlan.Plans.Any(p => p.DayPlanNum == 3) && weekPlan.Plans.Any(p => p.DayPlanNum == 4) && weekPlan.Plans.Any(p => p.DayPlanNum == 5) && weekPlan.Plans.Any(p => p.DayPlanNum == 6) && weekPlan.Plans.Any(p => p.DayPlanNum == 7))
                {
                    Have7DayPlan = true;
                }
            }
            if(Have7DayPlan && HaveMealPlan)
            {
                return true;
            }
            return false;
        }
        public ActionResult WorkoutPlanDelete(int id,int WeekPlanId,string UserId)
        {
            DeleteWorkoutPlanById(id);
            return RedirectToAction("ViewWeekPlan", new { WeekPlanId, UserId });
        }
        public void DeleteWorkoutPlanById(int id)
        {
            var workoutPlan = _context.WorkoutPlan.SingleOrDefault(m => m.Id == id);
            _context.Reps.RemoveRange(_context.Reps.Where(r => r.WorkoutPlanId == id));
            int PlanId = _context.WorkoutPlan.Where(w => w.Id == id).FirstOrDefault().PlanId;

            _context.WorkoutPlan.Remove(workoutPlan);
            _context.SaveChanges();
        }
        public ActionResult MakeWorkoutPlanRestDay(int WeekPlanId,string UserId,int PlanId)
        {
            Plan plan = _context.Plans.Where(p => p.Id == PlanId).FirstOrDefault();
            List<WorkoutPlan> workoutPlans = _context.WorkoutPlan.Where(w => w.PlanId == PlanId).ToList();
            foreach (var item in workoutPlans)
            {
                List<Reps> reps = _context.Reps.Where(r => r.WorkoutPlanId == item.Id).ToList();
                _context.Reps.RemoveRange(reps);
            }
            _context.WorkoutPlan.RemoveRange(workoutPlans);
            plan.Name = "休息日";
            _context.Plans.Update(plan);
            _context.SaveChanges();
            return RedirectToAction("ViewWeekPlan", new { WeekPlanId, UserId });
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