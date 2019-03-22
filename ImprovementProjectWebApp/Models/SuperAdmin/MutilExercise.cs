using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.SuperAdmin
{
    public class MutilExercise
    {
        [Required]
        [MaxLength(5000)]
        public string Exercises { get; set; }
        public int BodyPartId { get; set; }
    }
}
