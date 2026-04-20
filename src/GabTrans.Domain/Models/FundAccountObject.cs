using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FundAccountObject
    {
        public string AccountNumber { get; set; }
        public string AccountNumberType { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankCountry { get; set; }
        public string Currency { get; set; }
        public string PaymentType { get; set; }
        public long PaymentTypeId { get; set; }
        public string RoutingCode { get; set; }
        public string RoutingCodeType { get; set; }
    }
}
