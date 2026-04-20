using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransactionObject
    {
        public string? ReceiverAccountName { get; set; }
        public string? ReceiverAccountNumber { get; set; }
        public string? ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? Narration { get; set; }
        public string? Currency { get; set; }
        public string? InitiatedOn { get; set; }
    }
}
