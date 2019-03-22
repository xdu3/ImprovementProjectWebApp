using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class Plan
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Day Plan Name")]
        public string Name { get; set; }
        [Required]
        public DateTime DayPlanDate { get; set; }
        [Required]
        public int DayPlanNum { get; set; }

        [Display(Name = "WeekPlan")]
        public int WeekPlanId { get; set; }
        [ForeignKey("WeekPlanId")]
        public virtual WeekPlan WeekPlan { get; set; }

        [NotMapped]
        public int WorkoutNum { get; set; }
        //[Required]
        //[Display(Name = "AppUserPlan")]
        //public int AppUserPlanId { get; set; }
        //[ForeignKey("AppUserPlanId")]
        //public virtual AppUserPlan AppUserPlan { get; set; }
        public IEnumerable<WorkoutPlan> WorkoutPlans { get; set; }

    }
}
