using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class MemberListVM
    {
        public List<CustomerProfile> CustomerProfiles { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
