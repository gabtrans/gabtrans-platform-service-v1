using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Constants
{
    public static class GRemitStatuses
    {
        public const string Ready = "READY";
        public const string Payable = "PAYABLE";
        public const string Paying = "PAYING";
        public const string Paid = "PAID";
        public const string Error = "Error";
        public const string Rejected = "REJECTED";
        public const string Reversed = "REVERSED";
        public const string Cancelled = "CANCELLED";
        public const string Cancelling = "CANCELLING";
    }
}
