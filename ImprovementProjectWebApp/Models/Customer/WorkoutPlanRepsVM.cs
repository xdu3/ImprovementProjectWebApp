using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class WorkoutPlanRepsVM
    {
        public WorkoutPlan WorkoutPlan { get; set; }
        public List<Reps> Reps { get; set; }
    }
}
