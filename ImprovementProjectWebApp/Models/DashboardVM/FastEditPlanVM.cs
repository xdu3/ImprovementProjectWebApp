using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class FastEditPlanVM
    {
        public List<Plan> plans { get; set; }
        public int CopyPlanId { get; set; }
        public string UserId { get; set; }
        public List<WeekPlan> WeekPlans { get; set; }
    }
}
