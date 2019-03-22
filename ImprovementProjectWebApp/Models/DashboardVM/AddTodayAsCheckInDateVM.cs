using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class AddTodayAsCheckInDateVM
    {
        public string UserId { get; set; }
        public bool AddSuccess { get; set; }
        public bool AlreadyExist { get; set; }
    }
}
