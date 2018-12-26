using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class Exercise
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Part")]
        public int BodyPartId { get; set; }
        [ForeignKey("BodyPartId")]
        public virtual BodyPart BodyPart { get; set; }
    }
}
