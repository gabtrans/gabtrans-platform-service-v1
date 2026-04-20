using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BillPaymentDetails
    {
        public long Id { get; set; }
        public string? Package { get; set; }
        public string? RecipientName { get; set; }
        public string? Recipient { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string? Bill { get; set; }
        public string? TransactionStatus { get; set; }
        public string? Narration { get; set; }
        public string? Currency { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
