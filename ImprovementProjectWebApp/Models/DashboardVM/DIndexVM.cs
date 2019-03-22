using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class DIndexVM
    {
        public int CurUser { get; set; }
        public int UserWithoutPlan { get; set; }
        public int UserExpired { get; set; }
        public int UserAboutToExpired { get; set; }
        public int MealPlanRequired { get; set; }
        public int WorkoutPlanRequried { get; set; }
        public int CustomerNeedToContact { get; set; }
        public int ContactThem { get; set; }
        public int Qustions { get; set; }
    }
}
