using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class IntroQA
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Answer")]
        public string Answer { get; set; }
        [Display(Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Question")]
        public int IntroQuestionId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("IntroQuestionId")]
        public virtual IntroQuestion IntroQuestion { get; set; }
    }
}
