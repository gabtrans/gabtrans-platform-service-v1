using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class RechargeTV
    {
        public long AccountId { get; set; }
        public string SmartCardNumber { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string PackageCode { get; set; } = null!;
        //public string Package { get; set; } = null!;
        public string PayerName { get; set; } = null!;
        public long AccountKycType { get; set; }
        public string Provider { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;
        public string Request { get; set; } = null!;
        public long BillHistoryId { get; set; }
    }
}
