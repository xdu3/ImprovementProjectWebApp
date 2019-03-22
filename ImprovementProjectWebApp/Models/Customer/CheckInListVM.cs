using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class CheckInListVM
    {
        //public int CheckInQAId { get; set; }
        //public List<int> CheckInQuestionIds { get; set; }
        //public List<string> CheckInAnswers { get; set; }
        public List<CheckInQuestion> CheckInQuestions { get; set; }
        public List<CheckInQADetail> CheckInQADetails { get; set; }
    }
}
