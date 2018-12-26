using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class CheckInQADetail
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Required]
        [Display(Name = "Question")]
        public int CheckInQuestionId { get; set; }
        [ForeignKey("CheckInQuestionId")]
        public virtual CheckInQuestion CheckInQuestion { get; set; }
        [Required]
        [Display(Name = "CheckIn")]
        public int CheckInQAId { get; set; }
        [ForeignKey("CheckInQAId")]
        public virtual CheckInQA CheckInQA { get; set; }
    }
}
