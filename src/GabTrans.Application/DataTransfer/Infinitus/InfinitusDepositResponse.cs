using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusDepositResponse
    {
        public string id { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public double feeAmount { get; set; }
        public string feeCurrency { get; set; }
        public string reference { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string localAccountId { get; set; }
        public string payer { get; set; }
    }
}
