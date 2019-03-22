using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImprovementProjectWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using ImprovementProjectWebApp.Data;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TemplateController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IEmailSender _emailSender;
        //private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TemplateController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            //IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            //ILogger<AccountController> logger,
            ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _roleManager = roleManager;
            //_logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            string applicationUser = _context.ApplicationUser.Where(a => a.UserName == "dx3081@gmail.com").FirstOrDefault().Id;

            return RedirectToAction("ViewPlan", "Dashboard", new { id = applicationUser });
        }
    }
}