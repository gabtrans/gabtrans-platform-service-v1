using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BudPaySendMoney
    {
        public long PaymentTypeId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Narration { get; set; }
        public bool AddAsBen { get; set; }
        public long? BeneficiaryId { get; set; }
        public long UserId { get; set; }
        public long AccountId { get; set; }
        public long AccountKycType { get; set; }
        public string CountryCode { get; set; }
        public string BeneficiaryName { get; set; }
        //public string PayerName { get; set; }
        //public string PayerBank { get; set; }
        //public string PayerAccountNumber { get; set; }
        //public long ChannelId { get; set; }
        public decimal Fee { get; set; }
        public decimal TotalAmount { get; set; }
       // public string Payload { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;
    }
}
