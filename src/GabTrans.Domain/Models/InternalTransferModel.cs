using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class InternalTransferModel
    {
        public long PaymentTypeId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string SenderAccountNumber { get; set; }
        public string SenderAccountName { get; set; }
        public string Narration { get; set; }
        public long BeneficiaryId { get; set; }
        public long UserId { get; set; }
        public long ReceiverAccountId { get; set; }
        public long AccountId { get; set; }
        public long AccountKycType { get; set; }
        public string CountryCode { get; set; }
        public string BeneficiaryName { get; set; }
        public decimal Fee { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public string PayerEmail { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
