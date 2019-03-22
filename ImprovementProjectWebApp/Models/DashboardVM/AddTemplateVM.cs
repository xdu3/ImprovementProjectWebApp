using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class AddTemplateVM
    {
        public List<WeekPlan> weekPlans { get; set; }
        public int AppUserPlanId { get; set; }
        public String UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
