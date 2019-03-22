using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models
{
    public class UserCheckInDate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        [Display(Name = "AppUserPlan")]
        public int AppUserPlanId { get; set; }
        [ForeignKey("AppUserPlanId")]
        public virtual AppUserPlan AppUserPlan { get; set; }

    }
}
