using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.MemberViewModels
{
    public class MemberViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public CustomerProfile CustomerProfile { get; set; }
        public IList<AppUserPlan> UserPlans { get; set; } 
    }
}
