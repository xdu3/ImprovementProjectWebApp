using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Utility
{
    public static class SD
    {
        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "In Processing";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";

        public const string PaymentStatusPending = "未付款";
        public const string PaymentStatusApproved = "已付款";
        public const string PaymentStatusRejected = "付款未成功";
    }
}
