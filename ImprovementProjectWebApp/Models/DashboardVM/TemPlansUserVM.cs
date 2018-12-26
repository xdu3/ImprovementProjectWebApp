using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class TemPlansUserVM
    {
        public List<Plan> plans { get; set; }
        public string UserId { get; set; }
        public int AppUserPlanId { get; set; }
    }
}
