using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class ViewWeekPlanVM
    {
        public WeekPlan WeekPlan { get; set; }
        public int WeekPlanId { get; set; }
        public string UserId { get; set; }
        public int AppUserPlanId { get; set; }
    }
}
