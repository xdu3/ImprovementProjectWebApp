using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class WorkoutPlan
    {
        [Required]
        public int Id { get; set; }
        //[Required]
        //[Display(Name = "CreatedDate")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime CreatedDate { get; set; }
        //[Required]
        //[Display(Name = "Plan Name")]
        //public string PlanName { get; set; }
        //[Required]
        //[Display(Name = "Plan Track Id")]
        //public int PlanTrackId { get; set; }
        [Display(Name = "Sets")]
        public int Sets { get; set; }
        //[Display(Name = "Reps")]
        //public int Reps { get; set; }
        [Display(Name = "Other Type Exercise?")]
        public bool OtherTypeExercise { get; set; }
        [Display(Name = "Description")]
        public string Des { get; set; }
        [Display(Name = "Progressive Overload?")]
        public bool ProgressiveOverload { get; set; }

        [Required]
        [Display(Name = "Exercise Name")]
        public int ExerciseId { get; set; }
        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }


        [Display(Name = "Plan")]
        public int PlanId { get; set; }
        [ForeignKey("PlanId")]
        public virtual Plan Plan { get; set; }
        public IEnumerable<Reps> Reps { get; set; }
    }
}
