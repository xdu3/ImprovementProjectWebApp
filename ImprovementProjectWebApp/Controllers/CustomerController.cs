using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.Customer;
using ImprovementProjectWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin,Customer,Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IEmailSender _emailSender;
        public CustomerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            ApplicationUser CurApplicationUser = _context.ApplicationUser.Where(u => u.Id == curUserId).FirstOrDefault();
            if (CurApplicationUser.Birthday!= DateTime.MinValue)
            {
                AppUserPlanVM auser = new AppUserPlanVM();
                auser.appUserPlan = _context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Include(a => a.ApplicationUser).Where(a => a.StartDate.AddDays(-7) <= DateTime.Today && a.EndDate >= DateTime.Today).FirstOrDefault();

                auser.ApplicationUser = CurApplicationUser;

                auser.IfUserDelete = _context.ApplicationUser.Where(u => u.Id == curUserId).Select(u => u.IfDelete).FirstOrDefault();
                auser.IfUserEmailConfirmed = _context.ApplicationUser.Where(u => u.Id == curUserId).Select(u => u.EmailConfirmed).FirstOrDefault();
                if (_context.IntroQA.Any(i => i.UserId == curUserId))
                {
                    auser.IfHaveIntro = true;
                }
                int aaa = _context.IntroQA.Where(i => i.UserId == curUserId).Count();
                int bbb = _context.IntroQuestion.Where(i => i.IfHide == false).Count();
                if (_context.IntroQA.Where(i => i.UserId == curUserId).Count() == _context.IntroQuestion.Where(i => i.IfHide == false).Count())
                {
                    auser.IfFinishIntro = true;
                }
                if (_context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.IntroCheckInQA == true).Count() != 0)
                {
                    int CheckInQAId = _context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.IntroCheckInQA == true).FirstOrDefault().Id;
                    if (!_context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).Any(c => c.ImgURL == null))
                    {
                        auser.IfUploadImg = true;
                    }

                }

                return View(auser);
            }
            else
            {
                return RedirectToAction("AddBirthday");
            }
        }
        public ActionResult AddBirthday()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBirthday(DateTime Birthday)
        {
            
            string curUserId = _userManager.GetUserId(HttpContext.User);
            ApplicationUser CurApplicationUser = _context.ApplicationUser.Where(u => u.Id == curUserId).FirstOrDefault();
            CurApplicationUser.Birthday = Birthday;
            _context.ApplicationUser.Update(CurApplicationUser);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CheckWorkoutList(int PlanId)
        {

            List<WorkoutPlanRepsVM> WPVM = new List<WorkoutPlanRepsVM>();

            var x = _context.WorkoutPlan.Where(w => w.Plan.Id == PlanId).Include(w => w.Plan).Include(w => w.Exercise);
            foreach (var item in x)
            {
                WorkoutPlanRepsVM wpvm = new WorkoutPlanRepsVM();
                wpvm.Reps = _context.Reps.Where(r => r.WorkoutPlanId == item.Id).ToList();
                wpvm.WorkoutPlan = item;
                WPVM.Add(wpvm);
            }
            return View(WPVM);
        }
        public ActionResult CheckWorkoutList_V2(int PlanId)
        {
            Plan plan = _context.Plans.Where(p => p.Id == PlanId).Include(p => p.WorkoutPlans).ThenInclude(w => w.Reps).Include(p => p.WorkoutPlans).ThenInclude(w => w.Exercise).FirstOrDefault();
            CheckWorkoutList_V2VM checkWorkoutList_V2VM = new CheckWorkoutList_V2VM();
            checkWorkoutList_V2VM.Plan = plan;
            return View(checkWorkoutList_V2VM);

        }
        public ActionResult WorkoutList()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            int CurAppUserPlanId = _context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Where(a => a.StartDate <= DateTime.Today.AddDays(7) && a.EndDate >= DateTime.Today).FirstOrDefault().Id;//adddays 预防未来7天健身计划过期。、
            List<Plan> Plans = _context.Plans.Where(p=>p.WeekPlan.AppUserPlanId == CurAppUserPlanId).Include(p=>p.WeekPlan).ToList();
            return View(Plans);
        }


        public ActionResult AnswerIntroQA(int CurQId, int LastQId, int TargetQId)
        {

            string curUserId = _userManager.GetUserId(HttpContext.User);
            AnswerIntroQAVM answerIntroQA = new AnswerIntroQAVM();
            if (CurQId != 0)
            {
                answerIntroQA.CurQId = CurQId;
                answerIntroQA.IntroQA = _context.IntroQA.Where(i => i.UserId == curUserId).Where(i => i.IntroQuestionId == CurQId).FirstOrDefault();
                answerIntroQA.IntroQ = _context.IntroQuestion.Where(i => i.Id == CurQId).FirstOrDefault();
            }
            else
            {
                int a = _context.IntroQuestion.Where(i => i.IfHide == false).FirstOrDefault().Id;
                answerIntroQA.CurQId = a;
                answerIntroQA.IntroQA = _context.IntroQA.Where(i => i.UserId == curUserId).Where(i => i.IntroQuestionId == a).FirstOrDefault();
                answerIntroQA.IntroQ = _context.IntroQuestion.Where(i => i.Id == a).FirstOrDefault();
            }
            answerIntroQA.IntroQuestions = _context.IntroQuestion.Where(i => i.IfHide == false).ToList();
            answerIntroQA.IntroQAs = _context.IntroQA.Where(i => i.UserId == curUserId).ToList();

            answerIntroQA.CurQId = CurQId;
            answerIntroQA.LastQId = LastQId;
            answerIntroQA.TargetQId = TargetQId;

            return View(answerIntroQA);
        }


        public ActionResult AnswerIntroQAListView()
        {

            string curUserId = _userManager.GetUserId(HttpContext.User);
            AnswerIntroQAListVM answerIntroQAListVM = new AnswerIntroQAListVM();
            answerIntroQAListVM.IntroQuestions = _context.IntroQuestion.Where(i => i.IfHide == false).ToList();
            List<string> IntroAnswers = new List<string>();
            foreach(var item in answerIntroQAListVM.IntroQuestions)
            {
                string aaa = "";
                IntroAnswers.Add(aaa);
            }
            answerIntroQAListVM.IntroAnswers = IntroAnswers;
            return View(answerIntroQAListVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerIntroQAListView(AnswerIntroQAListVM answerIntroQAListVM)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);



            return View(answerIntroQAListVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerIntroQACreate(string QustionId, string Answer)
        {
            int QId = int.Parse(QustionId);

            IntroQA introQA = new IntroQA();
            if (QId == 0)
            {
                QId = _context.IntroQuestion.Where(i => i.IfHide == false).FirstOrDefault().Id;
            }
            if (_context.IntroQA.Where(i => i.UserId == _userManager.GetUserId(HttpContext.User)).Where(i => i.IntroQuestionId == QId).Count() != 0)
            {
                _context.IntroQA.RemoveRange(_context.IntroQA.Where(i => i.UserId == _userManager.GetUserId(HttpContext.User)).Where(i => i.IntroQuestionId == QId));
                _context.SaveChanges();
            }
            introQA.IntroQuestionId = QId;
            introQA.Answer = Answer.Trim();
            introQA.UserId = _userManager.GetUserId(HttpContext.User);
            introQA.CreatedDate = DateTime.Today;
            _context.IntroQA.Add(introQA);
            _context.SaveChanges();
            return RedirectToAction("AnswerIntroQA", new { CurQId = QId });
        }
        public ActionResult DeleteIntroQA(int IntroQAId)
        {
            int QId = _context.IntroQA.Where(i => i.Id == IntroQAId).FirstOrDefault().IntroQuestionId;
            _context.IntroQA.Remove(_context.IntroQA.Where(i => i.Id == IntroQAId).FirstOrDefault());
            _context.SaveChanges();
            return RedirectToAction("AnswerIntroQA", new { CurQId = QId });
        }

        public ActionResult CheckInView(int CheckInDetailId)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if (_context.UserCheckInDate.Where(u => u.AppUserPlan.ApplicationUserId == curUserId).Any(u => u.CheckInDate == DateTime.Today))
            {
                CheckInVM checkInVM = new CheckInVM();


                checkInVM.CheckInQADetail = _context.CheckInQADetail.Where(c => c.Id == CheckInDetailId).Include(c => c.CheckInQA).Include(c => c.CheckInQuestion).FirstOrDefault();


                checkInVM.CheckInQADetails = _context.CheckInQADetail.Where(c => c.CheckInQAId == checkInVM.CheckInQADetail.CheckInQA.Id).ToList();
                return View(checkInVM);
            }
            return View();
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnswerCheckInQ(string CheckInQADetailId, string Answer)
        {
            int CIQADId = int.Parse(CheckInQADetailId);
            CheckInQADetail checkInQADetail = _context.CheckInQADetail.Where(c => c.Id == CIQADId).FirstOrDefault();
            checkInQADetail.Answer = Answer;
            _context.CheckInQADetail.Update(checkInQADetail);
            _context.SaveChanges();


            return RedirectToAction("CheckInView", new { CheckInDetailId = CIQADId });
        }
        public ActionResult IfCheckIn()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if(_context.UserCheckInDate.Where(u=>u.AppUserPlan.ApplicationUserId == curUserId).Any(u=>u.CheckInDate == DateTime.Today))
            {
                var a = _context.UserCheckInDate.Where(u => u.AppUserPlan.ApplicationUserId == curUserId);
                var now = DateTime.Now;
                var today = DateTime.Today;
                return RedirectToAction("CheckInList", new { CheckInAvaiable = true });
            }
            else
            {
                return View();
            }
        }
        public ActionResult CheckInList()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if (_context.UserCheckInDate.Where(u => u.AppUserPlan.ApplicationUserId == curUserId).Any(u => u.CheckInDate == DateTime.Today ||u.CheckInDate.AddDays(1) == DateTime.Today))
            {
                var a = _context.UserCheckInDate.Where(u => u.AppUserPlan.ApplicationUserId == curUserId);
                var now = DateTime.Now;
                var today = DateTime.Today;
                CheckInQA checkInQA = new CheckInQA();
                if (_context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.CreatedDate == DateTime.Today).Where(c => c.IntroCheckInQA == false).Count() == 0)
                {

                    checkInQA.CreatedDate = DateTime.Today;
                    checkInQA.ApplicationUserId = curUserId;
                    _context.CheckInQA.Add(checkInQA);
                    _context.SaveChanges();
                }
                else
                {
                    checkInQA = _context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.CreatedDate == DateTime.Today).Where(c => c.IntroCheckInQA == false).FirstOrDefault();
                }
                if (!_context.CheckInQADetail.Any(C => C.CheckInQAId == checkInQA.Id))
                {
                    CheckInListVM checkInListVM = new CheckInListVM();
                    checkInListVM.CheckInQuestions = _context.CheckInQuestion.Where(c => c.IfHide == false).ToList();
                    List<CheckInQADetail> checkInQADetails = new List<CheckInQADetail>();
                    foreach (var item in checkInListVM.CheckInQuestions)
                    {
                        CheckInQADetail checkInQADetail = new CheckInQADetail();
                        //checkInQADetail.Answer = "";
                        checkInQADetail.CheckInQAId = checkInQA.Id;
                        checkInQADetail.CheckInQuestionId = item.Id;
                        checkInQADetails.Add(checkInQADetail);
                    }
                    checkInListVM.CheckInQADetails = checkInQADetails;
                    return View(checkInListVM);
                }
                else
                {
                    return RedirectToAction("UpLoadImgV2", new { CheckInQAId = checkInQA.Id });
                }
            }
            else
            {
                return RedirectToAction("CheckInNotAllowed");
            }

        }
        public ActionResult FinishedCheckIn(int CheckInQAId)
        {
            return View(_context.CheckInQA.Include(c => c.CheckInQADetails).Where(c => c.Id == CheckInQAId).FirstOrDefault());
        }
        public ActionResult CheckInNotAllowed()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInList(CheckInListVM checkInListVM)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            int CheckInQAId = checkInListVM.CheckInQADetails.FirstOrDefault().CheckInQAId;
            foreach (var item in checkInListVM.CheckInQADetails)
            {
                
                    _context.CheckInQADetail.Add(item);
                    _context.SaveChanges();
                
            }


            return RedirectToAction("UpLoadImgV2", new { CheckInQAId });

        }

        public ActionResult CheckIn()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if (_context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.CreatedDate == DateTime.Today).Where(c=>c.IntroCheckInQA == false).Count() == 0)
            {
                CheckInQA checkInQA = new CheckInQA();
                checkInQA.CreatedDate = DateTime.Today;
                checkInQA.ApplicationUserId = curUserId;
                _context.CheckInQA.Add(checkInQA);
                _context.SaveChanges();

                List<CheckInQADetail> checkInQADetails = new List<CheckInQADetail>();
                List<CheckInQuestion> checkInQuestions = _context.CheckInQuestion.Where(c => c.IfHide == false).ToList();

                        foreach (var item in checkInQuestions)
                        {
                            CheckInQADetail checkInQADetail = new CheckInQADetail();
                            checkInQADetail.CheckInQAId = checkInQA.Id;
                            checkInQADetail.CheckInQuestionId = item.Id;
                            _context.CheckInQADetail.Add(checkInQADetail);
                            _context.SaveChanges();
                        }

            }
            int firstId = _context.CheckInQADetail.Where(c=>c.CheckInQA.ApplicationUserId == curUserId).Where(c => c.CheckInQA.CreatedDate == DateTime.Today).Where(c => c.CheckInQA.IntroCheckInQA == false).FirstOrDefault().Id;
            return RedirectToAction("CheckInView", new { CheckInDetailId = firstId });

        }

        public ActionResult CheckInDetail(int CheckinQAId)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            CheckInDetailVM checkInDetailVM = new CheckInDetailVM();
            var checkInQAs = _context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.IntroCheckInQA == false).ToList();
            List<IfCheckInVM> ifCheckInVMs = new List<IfCheckInVM>();
            foreach (var item in checkInQAs)
            {
                IfCheckInVM ifCheckInVM = new IfCheckInVM();
                ifCheckInVM.CheckInQA = item;
                ifCheckInVM.IfCheckIn = _context.CheckInQADetail.Any(c => c.CheckInQAId == item.Id);
                ifCheckInVMs.Add(ifCheckInVM);
            }
            checkInDetailVM.ifCheckInVMs = ifCheckInVMs;
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
        public ActionResult InitialImg()
        {
            int CheckInQAId;
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if (!_context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Any(c => c.IntroCheckInQA == true))
            {
                CheckInQA checkInQA = new CheckInQA();
                checkInQA.ApplicationUserId = curUserId;
                checkInQA.CreatedDate = DateTime.Today;
                checkInQA.EndDate = DateTime.Today;
                checkInQA.IntroCheckInQA = true;
                _context.CheckInQA.Add(checkInQA);
                _context.SaveChanges();
                CheckInQAId = checkInQA.Id;
            }
            else
            {
                CheckInQAId = _context.CheckInQA.Where(c => c.ApplicationUserId == curUserId).Where(c => c.IntroCheckInQA == true).FirstOrDefault().Id;
            }
            return RedirectToAction("UpLoadImg", new { CheckInQAId });
        }
        public ActionResult UpLoadImg(int CheckInQAId, int CheckInImgsId)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if (_context.CheckInImgs.Any(c => c.CheckInQAId == CheckInQAId))
            {
                if (CheckInImgsId == 0)
                {
                    CheckInImgsVM checkInImgsVM = new CheckInImgsVM();
                    checkInImgsVM.CurCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).FirstOrDefault();
                    checkInImgsVM.AllCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).ToList();
                    return View(checkInImgsVM);
                }
                else
                {
                    CheckInImgsVM checkInImgsVM = new CheckInImgsVM();
                    checkInImgsVM.CurCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).Where(c => c.Id == CheckInImgsId).FirstOrDefault();
                    checkInImgsVM.AllCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).ToList();
                    return View(checkInImgsVM);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    CheckInImgs checkInImgs = new CheckInImgs();
                    checkInImgs.ImgPart = i;
                    checkInImgs.CheckInQAId = CheckInQAId;
                    _context.CheckInImgs.Add(checkInImgs);
                }
                _context.SaveChanges();
                CheckInImgsVM checkInImgsVM = new CheckInImgsVM();
                checkInImgsVM.CurCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).FirstOrDefault();
                checkInImgsVM.AllCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).ToList();
                return View(checkInImgsVM);


            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpLoadImgPost(int CheckInQAId, int CheckInImgsId)
        {
            if (ModelState.IsValid)
            {
                CheckInImgs checkInImgs = _context.CheckInImgs.Where(c => c.Id == CheckInImgsId).FirstOrDefault();
                string curUserId = _userManager.GetUserId(HttpContext.User);
                ApplicationUser CurUser = _context.ApplicationUser.Where(a => a.Id == curUserId).FirstOrDefault();
                var files = HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    string folderName = "Upload";
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    string userPath = Path.Combine(newPath, curUserId);
                    string SCheckInQAId = CheckInQAId + "";
                    string subFinder = Path.Combine(userPath, SCheckInQAId);
                    if (!Directory.Exists(subFinder))
                    {
                        Directory.CreateDirectory(subFinder);
                    }
                    if (files.Count != 1)
                    {
                        ModelState.AddModelError("", "You can only upload 1 file");
                        return RedirectToAction("UpLoadImgV2", new { CheckInQAId, CheckInImgsId });
                    }

                    IFormFile item = files[0];
                    if (item.Length > 0)
                    {
                        if (item.Length > 10 * 1024 * 1024)
                        //3mb = 3*1024
                        {
                            ModelState.AddModelError("", "You can't upload file more then 10MB");
                            return RedirectToAction("UpLoadImgV2", new { CheckInQAId, CheckInImgsId });
                        }


                        var extension = item.FileName.Substring(item.FileName.LastIndexOf("."), item.FileName.Length - item.FileName.LastIndexOf("."));
                        extension = extension.ToLower();
                        string[] fileType = { ".png", ".jpg", ".jpeg" };
                        if (!fileType.Contains(extension))
                        {
                            ModelState.AddModelError("", "You can only upload image files");
                            return RedirectToAction("UpLoadImgV2", new { CheckInQAId, CheckInImgsId });
                        }

                        //string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                        //string fileName = curUserId;
                        string fileName = "CheckInImg" + checkInImgs.Id + extension;//add a Id in the Name
                        string fullPath = Path.Combine(subFinder, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        checkInImgs.ImgURL = @"\Upload\" + curUserId + @"\" + SCheckInQAId + @"\" + fileName;
                        _context.CheckInImgs.Update(checkInImgs);
                        _context.SaveChanges();
                    }




                    return RedirectToAction("UpLoadImgV2", new { CheckInQAId, CheckInImgsId });
                }
                else
                {
                    return RedirectToAction("UpLoadImgV2", new { CheckInQAId, CheckInImgsId });
                }
            }
            return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
        }
        public ActionResult UpLoadImgV2(int CheckInQAId, int CheckInImgsId)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if (_context.CheckInImgs.Any(c => c.CheckInQAId == CheckInQAId))
            {
                CheckInImgsVM checkInImgsVM = new CheckInImgsVM();
                checkInImgsVM.AllCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).ToList();

                bool FinishUploaded = true;
                foreach(var i in checkInImgsVM.AllCheckInImgs)
                {
                    if(i.ImgURL == null)
                    {
                        FinishUploaded = false;
                    }
                }
                if(FinishUploaded)
                {
                    checkInImgsVM.CheckInQADetails = _context.CheckInQADetail.Include(c=>c.CheckInQuestion).Where(c => c.CheckInQAId == CheckInQAId).ToList();
                }



                return View(checkInImgsVM);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    CheckInImgs checkInImgs = new CheckInImgs();
                    checkInImgs.ImgPart = i;
                    checkInImgs.CheckInQAId = CheckInQAId;
                    _context.CheckInImgs.Add(checkInImgs);
                }
                _context.SaveChanges();
                CheckInImgsVM checkInImgsVM = new CheckInImgsVM();
                checkInImgsVM.AllCheckInImgs = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckInQAId).ToList();
                return View(checkInImgsVM);


            }
        }
            public ActionResult MealPlanList()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            int CurAppUserPlanId = _context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Where(a => a.StartDate <= DateTime.Today.AddDays(7) && a.EndDate >= DateTime.Today).FirstOrDefault().Id;//adddays 预防未来7天健身计划过期。
            List<MealPlan> mealPlans = _context.MealPlan.Where(p => p.WeekPlan.AppUserPlanId == CurAppUserPlanId).Include(p => p.WeekPlan).ToList();
            return View(mealPlans);
        }
        public ActionResult ViewCustomer()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            CustomerProfile customerProfile = _context.CustomerProfile.Where(c => c.ApplicationUserId == curUserId).Include(c => c.ApplicationUser).FirstOrDefault();
            return View(customerProfile);
        }
        public ActionResult ViewGallery()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            List<CheckInImgs> checkInImgs = _context.CheckInImgs.Where(c => c.CheckInQA.ApplicationUser.Id == curUserId).Include(c => c.CheckInQA).ThenInclude(c => c.ApplicationUser).ToList();
            return View(checkInImgs);
        }
        public ActionResult AddCustomerProfile()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            CustomerProfile customerProfile = new CustomerProfile();
            customerProfile.ApplicationUserId = curUserId;
            customerProfile.StartDate = DateTime.Today;
            return View(customerProfile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile customerProfile)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files[0] != null && files[0].Length > 0)
                {
                    //when user uploads an image
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    customerProfile.WeChatQRCode = p1;
                }
                _context.Add(customerProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(customerProfile);
        }

        public async Task<IActionResult> EditCustomerProfile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProfile = await _context.CustomerProfile.SingleOrDefaultAsync(m => m.Id == id);
            if (customerProfile == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", customerProfile.ApplicationUserId);
            return View(customerProfile);
        }

        // POST: CustomerProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomerProfile(int id, CustomerProfile customerProfile)
        {
            if (id != customerProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files[0] != null && files[0].Length > 0)
                    {
                        //when user uploads an image
                        byte[] p1 = null;
                        using (var fs1 = files[0].OpenReadStream())
                        {
                            using (var ms1 = new MemoryStream())
                            {
                                fs1.CopyTo(ms1);
                                p1 = ms1.ToArray();
                            }
                        }

                        customerProfile.WeChatQRCode = p1;
                    }
                    _context.Update(customerProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerProfileExists(customerProfile.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", customerProfile.ApplicationUserId);
            return View(customerProfile);
        }

        private bool CustomerProfileExists(int id)
        {
            return _context.CustomerProfile.Any(e => e.Id == id);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        public IActionResult AskProblem()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AskProblem( AskProblemVM askProblemVM)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            ApplicationUser applicationUser = _context.ApplicationUser.Where(a => a.Id == curUserId).Include(a=>a.CustomerProfiles).FirstOrDefault();
            var Email = applicationUser.Email;
            var UserName = applicationUser.UserName;
            var Name = applicationUser.CustomerProfiles.FirstOrDefault().Name;
            string message = "User Email: " + Email + "<br />" + "User Name: " + UserName + "<br />" + "Name: " + Name + "<br />"+"问题：" + askProblemVM.Question;
            _emailSender.SendEmailAsync("aesrev@outlook.com", "User Question-" + "Name", message);
            ViewData["Info"] = "问题已经被成功提交！谢谢您！";
            askProblemVM = new AskProblemVM();
            return View(askProblemVM);
        }
        public ActionResult EditQAD(int CheckInQAId)
        {
            CheckInListVM checkInListVM = new CheckInListVM();
            checkInListVM.CheckInQADetails = _context.CheckInQADetail.Where(c => c.CheckInQAId == CheckInQAId).ToList();
            checkInListVM.CheckInQuestions = _context.CheckInQuestion.Where(c => c.IfHide == false).ToList();
            return View(checkInListVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQAD(CheckInListVM checkInListVM)
        {

            string curUserId = _userManager.GetUserId(HttpContext.User);
            int CheckInQAId = checkInListVM.CheckInQADetails.FirstOrDefault().CheckInQAId;

            _context.CheckInQADetail.UpdateRange(checkInListVM.CheckInQADetails);
            _context.SaveChanges();

            return RedirectToAction("UpLoadImgV2", new { CheckInQAId });
        }
        public ActionResult DeleteCheckInPic(int CheckInImgsId,int CheckInQAId)
        {
            CheckInImgs checkInImgs = _context.CheckInImgs.Where(c => c.Id == CheckInImgsId).FirstOrDefault();
            var webRootPath = _hostingEnvironment.WebRootPath;
            var path = webRootPath + checkInImgs.ImgURL;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            checkInImgs.ImgURL = null;
            _context.CheckInImgs.Update(checkInImgs);
            _context.SaveChanges();
            return RedirectToAction("UpLoadImgV2", new { CheckInQAId });
        }
        public ActionResult  AddFeedbacks()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeedbacks(string Question)
        {
            FeedBack feedBack = new FeedBack();
            feedBack.ApplicationUserId = _userManager.GetUserId(HttpContext.User);
            feedBack.Qustion = Question;
            _context.FeedBack.Add(feedBack);
            _context.SaveChanges();
            return RedirectToAction("FeedbacksList");
        }
        public ActionResult FeedbacksList()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            FeedbacksListVM feedbacksListVM = new FeedbacksListVM();
            feedbacksListVM.FeedBacks = _context.FeedBack.Where(f => f.ApplicationUserId == curUserId).ToList();
            return View(feedbacksListVM);
        }

    }
}