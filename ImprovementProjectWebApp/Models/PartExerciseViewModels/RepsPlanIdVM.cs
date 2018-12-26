using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.PartExerciseViewModels
{
    public class RepsPlanIdVM
    {
        public int Set { get; set; }
        public int WorkoutPlanId { get; set; }
        public string PlanName { get; set; }
        public string UserId { get; set; }
    }
}
