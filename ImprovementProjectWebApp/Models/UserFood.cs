using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class UserFood
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "User")]
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        [Display(Name = "Food")]
        public int FoodItemId { get; set; }
        [ForeignKey("FoodCategoryId")]
        public virtual FoodItem FoodItem { get; set; }
    }
}
