using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class AppUserPlanVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public AppUserPlan appUserPlan { get; set; }
        //public IntroQA IntroQA { get; set; }
        public bool IfHaveIntro { get; set; }
        public bool IfFinishIntro { get; set; }
    }
}
