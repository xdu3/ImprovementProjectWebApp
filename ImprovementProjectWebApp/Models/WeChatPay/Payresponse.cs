using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.WeChatPay
{
    public class Payresponse
    {
        public string rsp_code { get; set; }
        public string rsp_msg { get; set; }
        public string data { get; set; }
        public string md5 { get; set; }
    }
}
