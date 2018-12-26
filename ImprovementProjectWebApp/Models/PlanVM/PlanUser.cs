using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.PlanVM
{
    public class PlanUser
    {
        public ApplicationUser User { get; set; }
        public Plan Plan { get; set; }
        public int AppUserPlanId { get; set; }
    }
}
