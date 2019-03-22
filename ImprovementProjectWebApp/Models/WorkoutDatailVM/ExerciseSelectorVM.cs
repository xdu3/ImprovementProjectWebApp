using ImprovementProjectWebApp.Models.PartExerciseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.WorkoutDatailVM
{
    public class ExerciseSelectorVM
    {

        public PartExercise PartExercise { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
        public List<PlanSetsReps> ListPlanSetsReps { get; set; }
        public bool IfTemplate { get; set; }
        public string UserID { get; set; }
        public int WeekPlanId { get; set; }
    }
}
