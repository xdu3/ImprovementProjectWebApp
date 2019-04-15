using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.User;
using ImprovementProjectWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
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
        public IActionResult Index(int CurPage,string Err,string Info,int FilterStatus,bool ShowDelete)
        {
            ViewData["Err"] = Err;
            ViewData["Info"] = Info;
            UserIndexVM userIndexVM = new UserIndexVM();
            
            userIndexVM.FilterStatus = FilterStatus;
            userIndexVM.ShowDelete = ShowDelete;
            var Ausers = _context.ApplicationUser.Include(a => a.CustomerProfiles);
            List<ApplicationUser> FinalUsers = new List<ApplicationUser>();
            if(ShowDelete == false)
            {
                var Ausers2 = Ausers.Where(a => a.IfDelete == false);
                if (FilterStatus == 1)
                {
                    //show email actived user
                    FinalUsers = Ausers2.Where(a => a.EmailConfirmed == true).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                }
                else
                {
                    FinalUsers = Ausers2.OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                }
            }
            else
            {
                var Ausers2 = Ausers;
                if (FilterStatus == 1)
                {
                    //show email actived user
                    FinalUsers = Ausers2.Where(a => a.EmailConfirmed == true).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                }
                else
                {
                    FinalUsers = Ausers2.OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList();
                }
            }
            


            int totalNum = FinalUsers.Count();
            var result = GetTotalPage(totalNum, 12, CurPage);
            if (result.Err == null)
            {

                userIndexVM.totalPage = result.TotalPage;
                //userIndexVM.applicationUsers = _context.ApplicationUser.Include(a => a.CustomerProfiles).OrderByDescending(a => a.CustomerProfiles.FirstOrDefault().StartDate).ToList().GetRange(result.StartPoint, result.Range);
                userIndexVM.applicationUsers = FinalUsers.GetRange(result.StartPoint, result.Range);
                userIndexVM.curPage = result.curPage;
                return View(userIndexVM);
            }
            else
            {
                userIndexVM.totalPage = 1;
                userIndexVM.applicationUsers = FinalUsers;
                userIndexVM.curPage =1;
                ViewData["Err"] = result.Err;
                return View(userIndexVM);
                
            }
        }
        public class PageResult
        {
            public int TotalPage { get; set; }
            public int StartPoint { get; set; }
            public int Range { get; set; }
            public int curPage { get; set; }
            public string Err { get; set; }
        }
        public static PageResult GetTotalPage(int totalNum, int NumberShowed, int CurPage)
        {
            int totalPage = 0;
            if (totalNum != 0)
            {
                if (totalNum < NumberShowed)
                {
                    totalPage = 1;
                }
                else
                {
                    if (totalNum % NumberShowed == 0)
                    {
                        totalPage = totalNum / NumberShowed;
                    }
                    else
                    {
                        totalPage = totalNum / NumberShowed + 1;
                    }
                }
                if(CurPage ==0)
                {
                    CurPage = 1;
                }
                if(CurPage>totalPage)
                {
                    CurPage = totalPage;
                }
                int StartPoint = (CurPage-1) * NumberShowed;
                int range = 0;
                if (CurPage == totalPage)
                {
                    
                    if(totalNum % NumberShowed == 0)
                    {
                        range = NumberShowed;
                    }
                    else
                    {
                        range = totalNum % NumberShowed;
                    }
                }
                else
                {
                    range = NumberShowed;
                }
                 var result = new PageResult
                {
                    TotalPage = totalPage,
                    StartPoint = StartPoint,
                    Range = range,
                    curPage = CurPage,
                    Err = null
                };
                return result;

            }
            else
            {
                var  result = new PageResult
                {
                    TotalPage = 0,
                    StartPoint = 0,
                    Range = 0,
                    curPage = CurPage,
                    Err = "Total number equal 0."
                };
                return result;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string searchString)
        {
            UserIndexVM userIndexVM = new UserIndexVM();
            
                var customers = from c in _context.ApplicationUser.Include(a => a.CustomerProfiles)
                                select c;

                if (!String.IsNullOrEmpty(searchString))
                {
                    customers = customers.Where(cstm => cstm.UserName.Contains(searchString) || cstm.Email.Contains(searchString)||cstm.CustomerProfiles.FirstOrDefault().Name.Contains(searchString));

                }
            else
            {
                userIndexVM.applicationUsers = _context.ApplicationUser.Include(a => a.CustomerProfiles).ToList();
            }

            userIndexVM.applicationUsers = customers.ToList();
           
            
            return View(userIndexVM);
        }
        public IActionResult CreateNewCustomer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewCustomer(CreateNewCustomerVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var result2 = await _userManager.AddToRoleAsync(user, "Customer");
                    //====================================================
                    if (result2.Succeeded)
                    {
                        user.EmailConfirmed = true;
                        _context.ApplicationUser.Update(user);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    AddErrors(result);
                }
                AddErrors(result);
            }
            return View(model);
        }
        public ActionResult ViewCustomerProfile(string UserId)
        {
            return View(_context.CustomerProfile.Where(c => c.ApplicationUserId == UserId).Include(c=>c.ApplicationUser).ThenInclude(c=>c.AppUserPlans).FirstOrDefault());
        }

        // GET: CustomerProfiles/Edit/5
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
        public async Task<IActionResult> EditCustomerProfile(int id,CustomerProfile customerProfile)
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

        // GET: CustomerProfiles/Delete/5
        public async Task<IActionResult> DeleteCustomerProfile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProfile = await _context.CustomerProfile
                .Include(c => c.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customerProfile == null)
            {
                return NotFound();
            }

            return View(customerProfile);
        }

        // POST: CustomerProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerProfile(int id)
        {
            var customerProfile = await _context.CustomerProfile.SingleOrDefaultAsync(m => m.Id == id);
            _context.CustomerProfile.Remove(customerProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult CreateCustomerProfile(string UserId)
        {
            CustomerProfile customerProfile = new CustomerProfile();
            customerProfile.ApplicationUserId = UserId;
            customerProfile.StartDate = DateTime.Today;
            return View(customerProfile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomerProfile( CustomerProfile customerProfile)
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

        public ActionResult DeleteUser(string UserID,int CurPage, bool ShowDelete)
        {
            var user = _context.ApplicationUser.Where(a => a.Id == UserID).FirstOrDefault();
            user.IfDelete = true;
            _context.ApplicationUser.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "User", new { CurPage, Info = "User Have Deleted" , ShowDelete });
        }
        public ActionResult RecoverUser(string UserID, int CurPage, bool ShowDelete)
        {
            var user = _context.ApplicationUser.Where(a => a.Id == UserID).FirstOrDefault();
            user.IfDelete = false;
            _context.ApplicationUser.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "User", new { CurPage, Info = "User Have Recovered" , ShowDelete });
        }
        public ActionResult ChangeUserPassword(string UserID)
        {
            ChangeUserPasswordVM changeUserPasswordVM = new ChangeUserPasswordVM();
            changeUserPasswordVM.UserId = UserID;
            return View(changeUserPasswordVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult ChangeUserPassword(ChangeUserPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _context.ApplicationUser.Where(a => a.Id == model.UserId).FirstOrDefault();
                if (user == null)
                {
                    return NotFound();
                }
                var userPassword = user.PasswordHash;
                var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                user.PasswordHash = newPassword;
                _context.ApplicationUser.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "User", new { Info = "Password changed successfully!" });                

            }
            return View(model);
        }
        public ActionResult ActiveEmail(string UserId)
        {
            var user = _context.ApplicationUser.Where(a => a.Id == UserId).FirstOrDefault();
            user.EmailConfirmed= true;
            _context.ApplicationUser.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "User", new { Info = "Email is actived" });
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
        
    }
}