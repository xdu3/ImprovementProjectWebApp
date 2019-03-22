using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class CheckInQA
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public bool IntroCheckInQA { get; set; }
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<CheckInQADetail> CheckInQADetails { get; set; }
        public IEnumerable<CheckInImgs> CheckInImgs { get; set; }
    }
}
