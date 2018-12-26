using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class CheckInVM
    {
        public CheckInQADetail CheckInQADetail { get; set; }
        public List<CheckInQADetail> CheckInQADetails { get; set; }
    }
}
