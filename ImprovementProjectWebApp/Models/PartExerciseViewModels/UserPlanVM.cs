using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.PartExerciseViewModels
{
    public class UserPlanVM
    {
        //public List<WorkoutPlan> WorkoutPlans { get; set; }
        public int count { get; set; }
        public List<string> PlanName { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Err { get; set; }
        public List<Plan> Plans { get; set; }
        public AppUserPlan AppUserPlan { get; set; }
    }
}
