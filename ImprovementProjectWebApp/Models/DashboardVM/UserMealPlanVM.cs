using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class UserMealPlanVM
    {
        public int AppUserPlanId { get; set; }
        public MealPlan MealPlan { get; set; }
    }
}
