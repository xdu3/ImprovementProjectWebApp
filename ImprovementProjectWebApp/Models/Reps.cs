using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class Reps
    {
        [Required]
        public int Id { get; set; }

        public int num { get; set; }

        [Required]
        [Display(Name = "WorkoutPlan")]
        public int WorkoutPlanId { get; set; }

        [ForeignKey("WorkoutPlanId")]
        public virtual WorkoutPlan WorkoutPlan { get; set; }
    }
}
