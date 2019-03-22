using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.Customer
{
    public class AnswerIntroQAListVM
    {
        public List<IntroQuestion> IntroQuestions { get; set; }
        public List<string> IntroAnswers { get; set; }
    }
}
