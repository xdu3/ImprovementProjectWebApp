using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class CheckInQuestion
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }

        public bool IfHide { get; set; }
    }
}
