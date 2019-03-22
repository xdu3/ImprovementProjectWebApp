using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.DashboardVM
{
    public class CheckInDetailVM
    {
        public List<CheckInQADetail> CheckInQADetails { get; set; }
        public List<IfCheckInVM> ifCheckInVMs { get; set; }
        public List<string> IMGUrl { get; set; }
        public IntroQA IntroQA { get; set; }
    }
}
