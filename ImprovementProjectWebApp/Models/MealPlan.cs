using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImprovementProjectWebApp.Models
{
    public class MealPlan
    {
        [Required]
        public int Id { get; set; }

        public string URL { get; set; }
        public bool Default { get; set; }
        [Display(Name = "WeekPlan")]
        public int WeekPlanId { get; set; }
        [ForeignKey("WeekPlanId")]
        public virtual WeekPlan WeekPlan { get; set; }
    }
}
