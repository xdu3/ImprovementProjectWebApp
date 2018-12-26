using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class CheckInImgsVM
    {
        public CheckInImgs CurCheckInImgs {get;set;}
        public List<CheckInImgs> AllCheckInImgs { get; set; }
    }
}
