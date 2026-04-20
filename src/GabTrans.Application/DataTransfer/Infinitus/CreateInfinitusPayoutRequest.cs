using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateInfinitusPayoutRequest
    {
        public string paymentCurrency { get; set; }
        public string sourceCurrency { get; set; }
        public string reference { get; set; }
        public string reason { get; set; }
        public string recipientId { get; set; }
        public string paymentMethod { get; set; }
        public string sourceAccountId { get; set; }
        public double paymentAmount { get; set; }
        public string? swiftChargeOption { get; set; }
    }
}
