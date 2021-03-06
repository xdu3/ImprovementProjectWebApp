﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.UserPlanViewModels;
using ImprovementProjectWebApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImprovementProjectWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var order = await _db.AppUserPlans.Include(a => a.ApplicationUser).Include(a => a.PlanPackage).FirstOrDefaultAsync(a => a.Id == id);

            return View(order);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<AppUserPlan> userPlans = await _db.AppUserPlans
                                                   .Include(o => o.ApplicationUser)
                                                   .Include(a => a.PlanPackage)
                                                   .Where(u => u.ApplicationUserId == claim.Value && u.PaymentStatus == SD.PaymentStatusApproved)
                                                   .ToListAsync();

            return View(userPlans);
        }

        [Authorize]
        public async Task<IActionResult> GetOrderDetails(int Id)
        {
            AppUserPlan userPlan = await _db.AppUserPlans
                                            .Include(a => a.ApplicationUser)
                                            .Include(a => a.PlanPackage)
                                            .FirstOrDefaultAsync(a => a.Id == Id);

            CustomerProfile profile = await _db.CustomerProfile.FirstOrDefaultAsync(c => c.ApplicationUserId == userPlan.ApplicationUserId);

            userPlan.UserName = profile.Name;
            userPlan.Phone = profile.PhoneNumber;


            return PartialView("_IndividualOrderDetails", userPlan);

        }

        [Authorize]
        public async Task<IActionResult> UserOrderList(string search_param, string search_text)
        {
            List<AppUserPlan> userPlans = await _db.AppUserPlans
                                                .Include(a => a.ApplicationUser)
                                                .Include(a => a.PlanPackage)
                                                .ToListAsync();

            if(search_param == "name")
            {
                userPlans = userPlans.Where(u => u.UserName.ToLower().Contains(search_text.ToLower())).ToList();
            }
            if(search_param == "email")
            {
                userPlans = userPlans.Where(u => u.ApplicationUser.Email.ToLower().Contains(search_text.ToLower())).ToList();
            }
            if(search_param == "phone")
            {
                userPlans = userPlans.Where(u => u.Phone.Contains(search_text)).ToList();
            }

            return View(userPlans);

        }

        [Authorize]
        public async Task<IActionResult> AddOrder(string userId)
        {
            if(userId == null)
            {
                return NotFound();
            }

            UserPlanViewModel userPlan = new UserPlanViewModel()
            {
                AppUserPlan = new Models.AppUserPlan(),
                ApplicationUser =await _db.CustomerProfile.Include(c => c.ApplicationUser).FirstOrDefaultAsync(c => c.ApplicationUserId == userId),
                AppUserPlans = await _db.AppUserPlans.Include(a => a.PlanPackage).Where(a => a.ApplicationUserId == userId).ToListAsync(),
                PlanPackages = await _db.PlanPackage.ToListAsync()
            };

            return View(userPlan);
        }

        [Authorize]
        public async Task<IActionResult> UserList(string search_param, string search_text)
        {

            var userList = await _db.CustomerProfile.Include(c => c.ApplicationUser).ToListAsync();

            if (search_param == "name")
            {
                userList = userList.Where(u => u.Name.ToLower().Contains(search_text.ToLower())).ToList();
            }
            if (search_param == "email")
            {
                userList = userList.Where(u => u.ApplicationUser.Email.ToLower().Contains(search_text.ToLower())).ToList();
            }
            if (search_param == "phone")
            {
                userList = userList.Where(u => u.PhoneNumber.Contains(search_text)).ToList();
            }

            return View(userList);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrder(UserPlanViewModel userPlan)
        {
            try
            {
                AppUserPlan appUserPlan = new AppUserPlan();
                appUserPlan = userPlan.AppUserPlan;
                appUserPlan.ApplicationUserId = userPlan.ApplicationUser.ApplicationUserId;
                appUserPlan.PaymentDate = DateTime.Now;
                appUserPlan.PaymentType = "Cash";
                appUserPlan.PlanPackageId = userPlan.PlanId;
                appUserPlan.PaymentStatus = SD.PaymentStatusApproved;
                appUserPlan.Status = SD.StatusCompleted;
                appUserPlan.UserName = userPlan.ApplicationUser.Name;
                appUserPlan.Phone = userPlan.ApplicationUser.PhoneNumber;


                await _db.AppUserPlans.AddAsync(appUserPlan);

                if (appUserPlan.StartDate.DayOfWeek == DayOfWeek.Monday)
                {
                    int period = ((TimeSpan)(appUserPlan.EndDate - appUserPlan.StartDate)).Days / 7;
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
                        await _db.UserCheckInDate.AddAsync(userCheckInDate);
                      
                    }
                }


                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(UserOrderList));
            }
            catch(Exception e)
            {
                userPlan.AppUserPlans = await _db.AppUserPlans.Include(a => a.PlanPackage).Where(a => a.ApplicationUserId == userPlan.ApplicationUser.ApplicationUserId).ToListAsync();
                userPlan.PlanPackages = await _db.PlanPackage.ToListAsync();

                return View(userPlan);
            }
            

           
        }


    }
}