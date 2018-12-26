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
            return View();
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

        public async Task<IActionResult> SearchCustomer(string searchString)
        {
            var customers = from c in _context.ApplicationUser
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.UserName.Contains(searchString));

            }

            return View(await customers.ToListAsync());
        }
        public ActionResult ViewCustomer(string id)
        {
            var customers = from c in _context.ApplicationUser
                            select c;

            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser customer = (customers.Where(c => c.Id == id)).FirstOrDefault();
                return View(customer);
            }
            return View();
        }

        public ActionResult ViewPlan(string id, string Err)
        {
            List<AppUserPlan> userPlans = _context.AppUserPlans.Include(a => a.ApplicationUser).Include(a => a.Plan).Include(a=>a.MealPlan).Where(a => a.ApplicationUserId == id).ToList();
            ViewData["Err"] = Err;
            ViewData["UserName"] = _context.ApplicationUser.Where(a=>a.Id == id).FirstOrDefault().UserName;
            ViewData["UserId"] = id;
            return View(userPlans);

        }
        public ActionResult ConnectToTem(string UserId, int AppUserPlanId)
        {
            
            TemPlansUserVM temPlansUserVM = new TemPlansUserVM();
            temPlansUserVM.plans = _context.Plans.Where(p => p.IfTemplate == true).ToList();
            temPlansUserVM.UserId = UserId;
            temPlansUserVM.AppUserPlanId = AppUserPlanId;
            return View(temPlansUserVM);

        }
        public ActionResult ViewAppUserPlan(int appUserPlanId)
        {
            return RedirectToAction("Details", "AppUserPlans",new { id = appUserPlanId });
        }
        //public ActionResult ViewUserCheckInQA(string UserId)
        //{
        //    return View(_context.CheckInQA.Where(c => c.UserId == UserId).Include(c=>c.ApplicationUser).ToList());
        //}
        public ActionResult AddPhasePlan(string UserId,int AppUserPlanId)
        {
            ViewData["AppUserPlanId"] = AppUserPlanId;
            return View(_context.ApplicationUser.Where(a => a.Id == UserId).FirstOrDefault());
        }
        public ActionResult TemConnect(string UserId, int PlanId,int AppUserPlanId)
        {
            if (_context.Plans.Any(p => p.Id == PlanId))
            {
                AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).Include(a=>a.ApplicationUser).FirstOrDefault();
                appUserPlan.PlanId = PlanId;
                _context.AppUserPlans.Update(appUserPlan);
                _context.SaveChanges();
                string send = SendEmail(appUserPlan.ApplicationUser.Email, "您有一个新的健身计划。");
                return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId });
            }
            else
            {
                return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId, Err = "The Plan Id doesn't exist in the database." });
            }
        }
        public ActionResult Disconnect(int appUserPlanId)
        {
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == appUserPlanId).FirstOrDefault();
            appUserPlan.PlanId = _context.Plans.Where(p => p.PlanName == "未完成").FirstOrDefault().Id;
            _context.AppUserPlans.Update(appUserPlan);
            _context.SaveChanges();
            return RedirectToAction("ViewPlan", "Dashboard", new { id = appUserPlan.ApplicationUserId });
        }
        public ActionResult DeleteListAppUserPlan(int TrackId, string UserId)
        {
            var ListAppUserPlan = _context.AppUserPlans.Where(a => a.TrackId == TrackId);
            _context.AppUserPlans.RemoveRange(ListAppUserPlan);
            _context.SaveChanges();
            return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId });
        }
        public ActionResult AddMealPlan(string UserId, int AppUserPlanId)
        {
            UserMealPlanVM userMealPlanVM = new UserMealPlanVM();
            userMealPlanVM.AppUserPlanId = AppUserPlanId;
            return View(userMealPlanVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMealPlan(int AppUserPlanId)
        {

            AppUserPlan CurUserPlan = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).Include(a=>a.ApplicationUser).FirstOrDefault();
            var files = HttpContext.Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                string userID = CurUserPlan.ApplicationUser.Id;
                string userPath = Path.Combine(newPath, userID);
                string MealPlan = "MealPlan";
                string userPath1 = Path.Combine(newPath, MealPlan);
                string subFinder = Path.Combine(userPath1, CurUserPlan.Id + "");
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
                    string[] fileType = { ".pdf" };
                    if (!fileType.Contains(extension))
                    {
                        ModelState.AddModelError("", "You can only upload image files");
                        //return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                    }

                    string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(subFinder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                    MealPlan mealPlan = new MealPlan();
                    mealPlan.URL = @"\Upload\MealPlan" + @"\" + CurUserPlan.Id + @"\" + fileName;

                    _context.MealPlan.Add(mealPlan);
                    _context.SaveChanges();
                    AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault();
                    appUserPlan.MealPlanId = mealPlan.Id;
                    _context.AppUserPlans.Update(appUserPlan);
                    _context.SaveChanges();
                    string send = SendEmail(CurUserPlan.ApplicationUser.Email,"您有一个新的饮食计划。");
                }
                
            }
            return RedirectToAction("ViewPlan", "Dashboard", new { id = CurUserPlan.ApplicationUserId });
        }
        public ActionResult ViewMealPlan(int AppUserPlanId)
        {
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault();
            MealPlan mealPlan = _context.MealPlan.Where(m => m.Id == appUserPlan.MealPlanId).FirstOrDefault();
            return View(mealPlan);
        }
        public ActionResult DeleteMealPlan(int AppUserPlanId,string UserId)
        {
            MealPlan mealPlan = _context.MealPlan.Where(m => m.Id == _context.AppUserPlans.Where(a => a.Id == AppUserPlanId).FirstOrDefault().MealPlanId).FirstOrDefault();
            mealPlan.URL = null;
            mealPlan.Default = true;
            //========删除文件还没有做====================
            _context.MealPlan.Update(mealPlan);
            _context.SaveChanges();
            return RedirectToAction("ViewPlan", "Dashboard", new { id = UserId });
        }

        public string SendEmail(string email,string bodyInfo)
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