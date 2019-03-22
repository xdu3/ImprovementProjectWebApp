using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class PlanPackage
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Des { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Term { get; set; }
    }
}
