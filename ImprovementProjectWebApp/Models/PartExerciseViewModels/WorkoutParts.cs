using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.PartExerciseViewModels
{
    public class WorkoutParts
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }

        [Display(Name = "Sets")]
        public int Sets { get; set; }
        [Display(Name = "Reps")]
        public int Reps { get; set; }
        [Display(Name = "Other Type Exercise?")]
        public bool OtherTypeExercise { get; set; }
        [Display(Name = "Description")]
        public string Des { get; set; }

        [Display(Name = "BodyPartId")]
        public string BodyPartId { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Exercise Name")]
        public int ExerciseId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; }
    }
}
