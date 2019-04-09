using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class PlanPackage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Des { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Price should be grater than ${1}")]
        [Required]
        public double Price { get; set; }
        [Required]
        public int Term { get; set; }
    }
}
