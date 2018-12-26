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

        [Required]
        [Display(Name = "AppUserPlan")]
        public int AppUserPlanId { get; set; }
        [ForeignKey("AppUserPlanId")]
        public virtual AppUserPlan AppUserPlan { get; set; }

    }
}
