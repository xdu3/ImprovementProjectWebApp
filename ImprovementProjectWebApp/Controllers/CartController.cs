using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Summary(int? planId)
        {
            if(planId == null)
            {
                return NotFound();
            }

            AppUserPlan userPlan = new AppUserPlan();

            userPlan.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();

            userPlan.ApplicationUser = applicationUser;
            userPlan.ApplicationUserId = claim.Value;

            var plan = await _db.PlanPackage.FindAsync(planId);

            if(plan == null)
            {
                return NotFound();
            }

            userPlan.PlanPackage = plan;
            userPlan.PlanPackageId = plan.Id;



            return View(userPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(string stripeEmail, string stripeToken, AppUserPlan userPlan)
        {

            var plan = await _db.PlanPackage.FindAsync(userPlan.PlanPackageId);
            userPlan.PlanPackage = plan;
            userPlan.OrderTotal = plan.Price;
            userPlan.PaymentType = "Stripe";
            userPlan.PaymentDate = DateTime.Now;

            await _db.AppUserPlans.AddAsync(userPlan);

            await _db.SaveChangesAsync();

            //Stripe Logic
            if (stripeToken != null)
            {
                var customers = new CustomerService();
                var charges = new ChargeService();

                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    SourceToken = stripeToken
                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(userPlan.OrderTotal * 100),
                    Description = "Order ID:" + userPlan.Id,
                    Currency = "cad",
                    CustomerId = customer.Id
                });

                userPlan.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {

                    //eamil for successful order
                    //await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == claim.Value).FirstOrDefault().Email, "Spice - Order Created " + detailCart.OrderHeader.Id.ToString(), "Order has been submitted successfully!");

                    userPlan.PaymentStatus = SD.PaymentStatusApproved;
                    userPlan.Status = SD.StatusSubmitted;
                }
                else
                {
                    userPlan.PaymentStatus = SD.PaymentStatusRejected;
                }

            }
            else
            {
                userPlan.PaymentStatus = SD.PaymentStatusRejected;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Confirm", "Order", new { id = userPlan.Id });
        }
    }
}