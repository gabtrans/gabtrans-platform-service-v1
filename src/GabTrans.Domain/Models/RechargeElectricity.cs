using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class RechargeElectricity
    {
        public long AccountId { get; set; }
        public string MeterNumber { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string PayerName { get; set; } = null!;
        public long AccountKycType { get; set; }
        public string Provider { get; set; } = null!;
        public string BillingType { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;
        public string Request { get; set; } = null!;
        public long TransactionId { get; set; }
    }
}
