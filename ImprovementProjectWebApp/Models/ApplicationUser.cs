using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ImprovementProjectWebApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool IfDelete { get; set; }
        public DateTime Birthday { get; set; }
        public IEnumerable<CustomerProfile> CustomerProfiles { get; set; }
        public IEnumerable<AppUserPlan> AppUserPlans { get; set; }
        public IEnumerable<IntroQA> IntroQAs { get; set; }
        public IEnumerable<CheckInQA> CheckInQAs { get; set; }
        public IEnumerable<UserCheckInDate> UserCheckInDates { get; set; }
        public IEnumerable<FeedBack> FeedBacks { get; set; }
    }
}
