using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.PartExerciseViewModels
{
    public class WorkoutExercise
    {
        public WorkoutPlan WorkoutPlan { get; set; }
        //public PlanSetsReps PlanSetsReps { get; set; }
        public PartExercise PartExercise { get; set; }
        public List<PlanSetsReps> ListPlanSetsReps { get; set; }
        public SetsReps setsReps { get; set; }
        public bool ProgressiveOverload { get; set; }
        public bool CreateBTNShow { get; set; }
    }
}
