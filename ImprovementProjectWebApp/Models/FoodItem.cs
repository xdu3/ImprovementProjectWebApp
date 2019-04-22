using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class FoodItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        public double Protein { get; set; }
        [Required]
        public double Carb { get; set; }
        [Required]
        public double Fat { get; set; }
        [Required]
        public double Calories { get; set; }
        [Required]
        public bool UserSelect { get; set; }
        [Required]
        [Display(Name = "Food Category")]
        public int FoodCategoryId { get; set; }
        [ForeignKey("FoodCategoryId")]
        public virtual FoodCategory FoodCategory { get; set; }
    }
}
