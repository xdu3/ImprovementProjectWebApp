using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class WeekPlan
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Week Plan Name")]
        public string Name { get; set; }
        [Required]
        public DateTime WeekPlanStartTime { get; set; }
        [Required]
        public DateTime WeekPlanEndTime { get; set; }
        [Required]
        [Display(Name = "AppUserPlan")]
        public int AppUserPlanId { get; set; }
        [ForeignKey("AppUserPlanId")]
        public virtual AppUserPlan AppUserPlan { get; set; }
        public IEnumerable<Plan> Plans { get; set; }
        public IEnumerable<MealPlan> MealPlan { get; set; }
    }
}
