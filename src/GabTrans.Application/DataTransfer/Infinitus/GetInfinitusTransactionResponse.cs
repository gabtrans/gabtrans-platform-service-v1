using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class GetInfinitusTransactionResponse
    {
        public bool hasMore { get; set; }
        public List<InfinitusTransactionItem> items { get; set; }
    }

    public class InfinitusTransactionItem
    {
        public string id { get; set; }
        public double amount { get; set; }
        public double clientRate { get; set; }
        public DateTime createdAt { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public double fee { get; set; }
        public string sourceId { get; set; }
        public string sourceType { get; set; }
        public string status { get; set; }
        public string transactionType { get; set; }
    }
}
