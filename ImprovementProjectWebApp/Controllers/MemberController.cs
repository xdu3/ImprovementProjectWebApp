using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImprovementProjectWebApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MemberController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> Index(string search_param, string search_text)
        {
          
            var memberId = await _db.AppUserPlans.Select(a => a.ApplicationUserId).ToListAsync();
            var memberList = await _db.CustomerProfile.Include(c => c.ApplicationUser).Where(c => memberId.Contains(c.ApplicationUserId) && c.ApplicationUser.IfDelete == false).ToListAsync();



            List<MemberViewModel> memberVM = new List<MemberViewModel>();
  

            if (search_param == "name")
            {
                memberList = memberList.Where(u => u.Name.ToLower().Contains(search_text.ToLower())).ToList();
            }
            if (search_param == "email")
            {
                memberList = memberList.Where(u => u.ApplicationUser.Email.ToLower().Contains(search_text.ToLower())).ToList();
            }
            if (search_param == "phone")
            {
                memberList = memberList.Where(u => u.PhoneNumber.Contains(search_text)).ToList();
            }

            foreach(var member in memberList)
            {
                MemberViewModel model = new MemberViewModel();

                model.CustomerProfile = member;
                model.ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(a => a.Id == member.ApplicationUserId);
                model.UserPlans = await _db.AppUserPlans.Include(a => a.PlanPackage).Where(a => a.ApplicationUserId == member.ApplicationUserId).ToListAsync();

                memberVM.Add(model);

            }


            return View(memberVM);
        }
    }
}