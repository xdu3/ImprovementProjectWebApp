using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.WeChatPay
{
    public class PayCodeViewModel
    {
        public AppUserPlan AppUserPlan { get; set; }
        public byte[] qrImage { get; set; }
        public string OrderId { get; set; }
    }
}
