using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransferReceiptObject
    {
        public string AccountName { get; set; }
        public string WalletNumber { get; set; }
        public string PaymentType { get; set; }
        public string TransactionStatus { get; set; }
        public string Amount { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public string TransactionSource { get; set; }
        public string Remark { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public string BeneficiaryBank { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        public string RoutingCode { get; set; }
        public long PaymentTypeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Payment { get; set; }
        public string PaymentOption { get; set; }
        public string FullName { get; set; }
        public string TransactionReference { get; set; }
        public string TransactionType { get; set; }
        public string ModuleName { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public string TransactionTime { get; set; }
        public string Narration { get; set; }
        public string AmountInWords { get; set; }
    }
}
