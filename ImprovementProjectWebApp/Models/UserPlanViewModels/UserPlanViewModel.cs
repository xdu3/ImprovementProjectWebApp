using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.UserPlanViewModels
{
    public class UserPlanViewModel
    {
        public AppUserPlan AppUserPlan { get; set; }
        public CustomerProfile ApplicationUser { get; set; }
        public int PlanId { get; set; }

        public IEnumerable<AppUserPlan> AppUserPlans { get; set; }
        public IList<PlanPackage> PlanPackages { get; set; }
    }
}
