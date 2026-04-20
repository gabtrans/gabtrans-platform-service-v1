using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class LimitRequest
    {
        public long AccountId { get; set; }

        public decimal SingleCumulative { get; set; }

        public long DailyCount { get; set; }

        public string Currency { get; set; }

        public string? TransactionType { get; set; }

        public string AccountType { get; set; }

        public decimal DailyCumulative { get; set; }
    }
}
