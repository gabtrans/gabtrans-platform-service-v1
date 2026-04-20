using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class Receipt
    {
        public long TransactionId { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public long? SenderAccountId { get; set; }
        public string? SenderAccountNumber { get; set; }
        public string? SenderAccountName { get; set; }
        public string? SenderBank { get; set; }
        public long? ReceiverAccountId { get; set; }
        public string ReceiverAccountNumber { get; set; } = null!;
        public string? ReceiverAccountName { get; set; }
        public string? ReceiverBank { get; set; }
        public decimal Amount { get; set; }
        public decimal SettledAmount { get; set; }
        public decimal Fee { get; set; }
        public long Status { get; set; }
        public string TransactionStatus { get; set; }
        public string Currency { get; set; } = null!;
        public string Narration { get; set; } = null!;
        public string? InitialRequest { get; set; }
        public string? InitialResponse { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public DateTime DateCreated { get; set; }
        public long BillId { get; set; }
        public string? Bill { get; set; }
        public long TransactionTypeId { get; set; }
        public string TransactionType { get; set; } = null!;
        public long PaymentOptionId { get; set; }
        public string SenderCountryCode { get; set; } = null!;
        public string? ReceiverCountryCode { get; set; }
        public string? RoutingCode { get; set; }
        public string? RoutingCodeType { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string? TranReference { get; set; }
        public string? SenderCurrency { get; set; }
        public long PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public string CountryCode { get; set; } = null!;
        public string Template { get; set; } = null!;
        public string Provider { get; set; }
        public string Package { get; set; }
        public string AccountType { get; set; }
        public string CreditToken { get; set; }
        public string Units { get; set; }
    }
}
