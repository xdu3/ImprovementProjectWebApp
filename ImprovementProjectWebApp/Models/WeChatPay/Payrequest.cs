using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.WeChatPay
{
    public class Payrequest
    {
        public string action { get; set; }
        public string version { get; set; }
        public string merchant_id { get; set; }
        public string md5 { get; set; }
        public string data { get; set; }
    }
}
