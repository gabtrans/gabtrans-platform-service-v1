using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BillPaymentReceipt
    {
        public string SenderEmailAddress { get; set; }
        public string SenderFullName { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string CustomerName { get; set; }
        public string PackageName { get; set; }
        public string AccountType { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ProductCategory { get; set; }
        public string WalletNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string TransactionDate { get; set; }
        public string ProductName { get; set; }
        public string Recipient { get; set; }
        public string ReferenceNumber { get; set; }
        public string Currency { get; set; }
        public string TransactionTime { get; set; }
        public string Narration { get; set; }
        public string BillsType { get; set; }
        public string Units { get; set; }
        public string Token { get; set; }
        public List<string> ChangeToken { get; set; } = new List<string>();
        public string Attachment { get; set; }
        public string Description { get; set; }
        public string BeneficiaryBank { get; set; }
    }
}
