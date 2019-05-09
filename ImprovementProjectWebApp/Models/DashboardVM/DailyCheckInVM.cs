using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class DailyCheckInVM
    {
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationUser SelectUser { get; set; }
        public DateTime Date { get; set; }
    }
}
