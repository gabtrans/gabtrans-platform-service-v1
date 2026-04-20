using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ReceiveMoneyObject
    {
        public string Currency { get; set; }
        public string Recipient { get; set; }
        public string RecipientName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
        public string RoutingCode { get; set; }
        public string BankAddress { get; set; }
        public string BankCountry { get; set; }
        public long PaymentTypeId { get; set; }
        public long UserId { get; set; }
    }
}
