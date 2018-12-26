using System.ComponentModel.DataAnnotations;

namespace ImprovementProjectWebApp.Models
{
    public class MealPlan
    {
        [Required]
        public int Id { get; set; }

        public string URL { get; set; }
        public bool Default { get; set; }
    }
}
