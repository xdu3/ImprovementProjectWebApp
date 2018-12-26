using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class Plan
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }
        public bool Active { get; set; }
        public bool IfTemplate { get; set; }


    }
}
