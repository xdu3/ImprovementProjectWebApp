using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.WeChatPay
{
    public class Paydata
    {
        public string order_id { get; set; }
        public string call_back_url { get; set; }
        public string biz_type { get; set; }
        public string operator_id { get; set; }
        public string amount { get; set; }
    }
}
