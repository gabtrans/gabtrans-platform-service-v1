using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransactionHistory
    {
        public long Id { get; set; }
        public string? SenderAccountName { get; set; }
        public string? ReceiverAccountName { get; set; }
        public string? SenderAccountNumber { get; set; }
        public string? ReceiverAccountNumber { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? DateUpdated { get; set; }
        public decimal Amount { get; set; }
        public decimal SettledAmount { get; set; }
        public decimal Fee { get; set; }
        public string? SenderBank { get; set; }
        public string? ReceiverBank { get; set; }
        public string? Category { get; set; }
        public string? PaymentOption { get; set; }
        public string? PaymentType { get; set; }
        public string? TransactionStatus { get; set; }
        public string? Narration { get; set; }
        public string? Currency { get; set; }
        public string? RoutingCode { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Suspicious { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}
