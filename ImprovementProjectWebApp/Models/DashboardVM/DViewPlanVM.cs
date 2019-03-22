using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class DViewPlanVM
    {
        public AppUserPlan AppUserPlan { get; set; }
        public List<WeekPlan> WeekPlans { get; set; }
        public List<Plan> Plans { get; set; }
        public int WeekLeft { get; set; }
        public int SelectWeekPlansId { get; set; }
        public bool MealPlanExist { get; set; }
        public string MealPlanURL { get; set; }
        public string CheckInStatus { get; set; }
        public bool IfLastWeekPlan { get; set; }
        public bool IfTemplate { get; set; }
    }
}
