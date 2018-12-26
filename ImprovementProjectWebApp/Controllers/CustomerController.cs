using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public CustomerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
        }
        //public async Task<IActionResult> IndexAsync()
        //{
        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        //    return View(user);
        //}
        public ActionResult Index()
        {
            string curUserId =  _userManager.GetUserId(HttpContext.User);
            AppUserPlanVM auser = new AppUserPlanVM();
            auser.appUserPlan = _context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Include(a => a.ApplicationUser).Include(a => a.Plan).FirstOrDefault();
            auser.ApplicationUser = _context.ApplicationUser.Where(u => u.Id == curUserId).FirstOrDefault();
            
            if(_context.IntroQA.Any(i => i.UserId == curUserId))
            {
                auser.IfHaveIntro = true;
            }
            if(_context.IntroQA.Where(i => i.UserId == curUserId).Count() == _context.IntroQuestion.Where(i=>i.IfHide == false).Count())
            {
                auser.IfFinishIntro = true;
            }
            return View(auser);
        }
        public ActionResult CheckWorkoutList( int PlanId)
        {
            //string curUserId = _userManager.GetUserId(HttpContext.User);
            //int planID = _context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Include(a => a.ApplicationUser).Include(a => a.Plan).FirstOrDefault().PlanId;
            
            List<WorkoutPlanRepsVM> WPVM = new List<WorkoutPlanRepsVM>();
            var x = _context.WorkoutPlan.Where(w => w.Plan.Id == PlanId).Include(w => w.Plan).Include(w => w.Exercise);
            foreach(var item in x )
            {
                WorkoutPlanRepsVM wpvm = new WorkoutPlanRepsVM();
                wpvm.Reps = _context.Reps.Where(r => r.WorkoutPlanId == item.Id).ToList();
                wpvm.WorkoutPlan = item;
                WPVM.Add(wpvm);
            }
            return View(WPVM);
        }
        public ActionResult WorkoutList()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            return View(_context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Include(a => a.ApplicationUser).Include(a => a.Plan).ToList());
        }
        //public ActionResult WorkoutList()
        //{
        //    string curUserId = _userManager.GetUserId(HttpContext.User);
        //    return View(_context.AppUserPlans.Where(u => u.ApplicationUserId == curUserId).Include(a => a.ApplicationUser).Include(a => a.Plan).ToList());
        //}
        //public ActionResult AnswerIntroQA()
        //{
        //    List<IntroQuestion> introQuestions = _context.IntroQuestion.Where(i=>i.IfHide == false).ToList();
        //    return View(introQuestions);
        //}

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


        [HttpPost]
        public ActionResult AnswerIntroQACreate(string QustionId, string Answer)
        {
            int QId = int.Parse(QustionId);
            IntroQA introQA = new IntroQA();
            if(QId == 0)
            {
                QId = _context.IntroQuestion.Where(i => i.IfHide == false).FirstOrDefault().Id;
            }
            if(_context.IntroQA.Where(i=>i.UserId == _userManager.GetUserId(HttpContext.User)).Where(i=>i.IntroQuestionId == QId).Count()!= 0)
            {
                _context.IntroQA.RemoveRange(_context.IntroQA.Where(i => i.UserId == _userManager.GetUserId(HttpContext.User)).Where(i => i.IntroQuestionId == QId));
                _context.SaveChanges();
            }
            introQA.IntroQuestionId = QId;
            introQA.Answer = Answer;
            introQA.UserId= _userManager.GetUserId(HttpContext.User);
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
            CheckInVM checkInVM = new CheckInVM();


            checkInVM.CheckInQADetail = _context.CheckInQADetail.Where(c => c.Id == CheckInDetailId).Include(c => c.CheckInQA).Include(c => c.CheckInQuestion).FirstOrDefault();
            

            checkInVM.CheckInQADetails = _context.CheckInQADetail.Where(c => c.CheckInQAId == checkInVM.CheckInQADetail.CheckInQA.Id).ToList();

            return View(checkInVM);
        }
        [HttpPost]
        public ActionResult AnswerCheckInQ(string CheckInQADetailId, string Answer)
        {
            int CIQADId = int.Parse(CheckInQADetailId);
            CheckInQADetail checkInQADetail = _context.CheckInQADetail.Where(c => c.Id == CIQADId).FirstOrDefault();
            checkInQADetail.Answer = Answer;
            _context.CheckInQADetail.Update(checkInQADetail);
            _context.SaveChanges();


            return RedirectToAction("CheckInView", new { CheckInDetailId = CIQADId });
        }
        public int GetCheckIn(string userId)
        {
            AppUserPlan appUserPlan = _context.AppUserPlans.Where(a=>a.ApplicationUserId == userId).Where(a => a.StartDate <= DateTime.Today && a.EndDate >= DateTime.Today).FirstOrDefault();
            //get current a>ppUserPlan
            if (appUserPlan != null)
            {
                if (!_context.CheckInQA.Any(c => c.AppUserPlanId == appUserPlan.Id))
                {
                    DateTime dt1 = new DateTime();
                    for (int i = 0; i < 4; i++)
                    {
                        CheckInQA checkInQA = new CheckInQA();
                        checkInQA.CreatedDate = appUserPlan.StartDate;
                        checkInQA.EndDate = appUserPlan.StartDate.AddDays(7);
                        dt1 = appUserPlan.StartDate.AddDays(7);
                        checkInQA.AppUserPlanId = appUserPlan.Id;
                        _context.CheckInQA.Add(checkInQA);
                        _context.SaveChanges();
                    }
                }

                CheckInQA CurrentCheckInQA = _context.CheckInQA.Where(c => c.AppUserPlanId == appUserPlan.Id).Where(a => a.CreatedDate <= DateTime.Today && a.EndDate >= DateTime.Today).FirstOrDefault();
                CurrentCheckInQA.Active = true;
                _context.CheckInQA.Update(CurrentCheckInQA);
                _context.SaveChanges();
                
                return (CurrentCheckInQA.Id);


            }
            else
            {
                return (-999);
            }
        }
        public ActionResult CheckIn()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            int CurrentCheckInQAId = GetCheckIn(curUserId);
            if (CurrentCheckInQAId != -999)
            {
                if (!(_context.CheckInQADetail.Any(c => c.CheckInQAId == CurrentCheckInQAId)))
                {
                    List<CheckInQADetail> checkInQADetails = new List<CheckInQADetail>();
                    List<CheckInQuestion> checkInQuestions = _context.CheckInQuestion.Where(c => c.IfHide == false).ToList();

                    foreach (var item in checkInQuestions)
                    {
                        CheckInQADetail checkInQADetail = new CheckInQADetail();
                        checkInQADetail.CheckInQAId = CurrentCheckInQAId;
                        checkInQADetail.CheckInQuestionId = item.Id;
                        _context.CheckInQADetail.Add(checkInQADetail);
                        _context.SaveChanges();
                    }
                }
            }

            int firstId = _context.CheckInQADetail.Where(c => c.CheckInQAId == CurrentCheckInQAId).FirstOrDefault().Id;
            return RedirectToAction("CheckInView", new { CheckInDetailId = firstId });

        }
        public ActionResult CheckInDetail( int CheckinQAId)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            CheckInDetailVM checkInDetailVM = new CheckInDetailVM();
            var checkInQAs = _context.CheckInQA.Where(c => c.AppUserPlan.ApplicationUserId == curUserId).ToList();
            List<IfCheckInVM> ifCheckInVMs = new List<IfCheckInVM>();
            foreach(var item in checkInQAs)
            {
                IfCheckInVM ifCheckInVM = new IfCheckInVM();
                ifCheckInVM.CheckInQA = item;
                ifCheckInVM.IfCheckIn =  _context.CheckInQADetail.Any(c => c.CheckInQAId == item.Id);
                ifCheckInVMs.Add(ifCheckInVM);
            }
            checkInDetailVM.ifCheckInVMs = ifCheckInVMs;
            if(CheckinQAId!= 0)
            {
                if (_context.CheckInQADetail.Any(c => c.CheckInQAId == CheckinQAId))
                {
                    checkInDetailVM.CheckInQADetails = _context.CheckInQADetail.Where(c => c.CheckInQAId == CheckinQAId).Include(c => c.CheckInQuestion).ToList();
                }

                checkInDetailVM.IMGUrl = _context.CheckInImgs.Where(c => c.CheckInQAId == CheckinQAId).Select(c => c.ImgURL).ToList();

            }
            return View(checkInDetailVM);
        }

        public ActionResult UpLoadImg(int CheckInQAId, int CheckInImgsId)
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            if(_context.CheckInImgs.Any(c=>c.CheckInQAId == CheckInQAId))
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
                for(int i=0; i<4;i++)
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
                        return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                    }

                        IFormFile item = files[0];
                        if (item.Length > 0)
                        {
                            if (item.Length > 10 * 1024 * 1024)
                            //3mb = 3*1024
                            {
                                ModelState.AddModelError("", "You can't upload file more then 10MB");
                                return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                            }
                            

                            var extension = item.FileName.Substring(item.FileName.LastIndexOf("."), item.FileName.Length - item.FileName.LastIndexOf("."));
                            extension = extension.ToLower();
                            string[] fileType = { ".png", ".jpg", ".jpeg" };
                            if (!fileType.Contains(extension))
                            {
                                ModelState.AddModelError("", "You can only upload image files");
                                return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                            }

                            string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                            string fullPath = Path.Combine(subFinder, fileName);

                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }

                        checkInImgs.ImgURL = @"\Upload\" + curUserId + @"\" + SCheckInQAId + @"\" + fileName;
                        _context.CheckInImgs.Update(checkInImgs);
                        _context.SaveChanges();
                        }

                        


                    return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                }
                else
                {
                    return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
                }
            }
            return RedirectToAction("UpLoadImg", new { CheckInQAId, CheckInImgsId });
        }
        public ActionResult ViewMealPlan()
        {
            string curUserId = _userManager.GetUserId(HttpContext.User);
            List<AppUserPlan> appUserPlans = _context.AppUserPlans.Where(a => a.ApplicationUserId == curUserId).Where(a=>a.MealPlan.URL != null).Include(a=>a.MealPlan).Include(a=>a.Plan).ToList();
            return View(appUserPlans);

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
            List<CheckInImgs> checkInImgs = _context.CheckInImgs.Where(c => c.CheckInQA.AppUserPlan.ApplicationUser.Id == curUserId).Include(c=>c.CheckInQA).ThenInclude(c=>c.AppUserPlan).ThenInclude(c=>c.ApplicationUser).ToList();
            return View(checkInImgs);
        }

    }
}