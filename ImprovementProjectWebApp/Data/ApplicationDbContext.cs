using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ImprovementProjectWebApp.Models;

namespace ImprovementProjectWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<FeedBack> FeedBack { get; set; }
        public DbSet<CustomerProfile> CustomerProfile { get; set; }
        public DbSet<BodyPart> BodyPart { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlan { get; set; }
        public DbSet<CheckInQuestion> CheckInQuestion { get; set; }
        public DbSet<IntroQuestion> IntroQuestion { get; set; }
        public DbSet<IntroQA> IntroQA { get; set; }
        public DbSet<CheckInQA> CheckInQA { get; set; }
        public DbSet<CheckInQADetail> CheckInQADetail { get; set; }
        public DbSet<Reps> Reps { get; set; }
        public DbSet<AppUserPlan> AppUserPlans { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<CheckInImgs> CheckInImgs { get; set; }
        public DbSet<MealPlan> MealPlan { get; set; }
        public DbSet<WeekPlan> WeekPlan { get; set; }
        public DbSet<UserCheckInDate> UserCheckInDate { get; set; }
        public DbSet<PlanPackage> PlanPackage { get; set; }
        public DbSet<FoodCategory> FoodCategory { get; set; }
        public DbSet<FoodItem> FoodItem { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
