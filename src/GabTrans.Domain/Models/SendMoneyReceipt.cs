using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class SendMoneyReceipt
    {
        public string PaymentType { get; set; }
        public string PaymentDays { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string BeneficiaryName { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public string ABA { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string BSBCode { get; set; }
        public string InstitutionNo { get; set; }
        public string BankCode { get; set; }
        public string BranchCode { get; set; }
        public string PaymentReference { get; set; }
        public string Status { get; set; }
        public string CountryCode { get; set; }
        public string Bank { get; set; }
        public string SenderEmailAddress { get; set; }
        public string SenderFirstName { get; set; }
        public string TemplateId { get; set; }
        public string TransactionDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string ShortReference { get; set; }
        public string Description { get; set; }
    }
}
