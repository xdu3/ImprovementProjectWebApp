using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class AnswerIntroQAVM
    {
        public List<IntroQuestion> IntroQuestions { get; set; }
        public List<IntroQA> IntroQAs { get; set; }
        public IntroQA IntroQA { get; set; }
        public int LastQId { get; set; }
        public int TargetQId { get; set; }
        public int CurQId { get; set; }
        public IntroQuestion IntroQ { get; set; }
    }
}
