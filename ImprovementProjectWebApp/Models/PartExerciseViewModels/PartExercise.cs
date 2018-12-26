using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.PartExerciseViewModels
{
    public class PartExercise
    {
        public List<Exercise> Exercises { get; set; }
        public List<BodyPart> BodyParts { get; set; }
        public int SelectExerciseId { get; set; }
        public int SelectPartId { get; set; }
        public int AppUserPlanId { get; set; }
        public int WorkoutPlanId { get; set; }
    }
}
