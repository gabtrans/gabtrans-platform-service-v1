using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class PaymentDeliveryDates
    {
        public string payment_date { get; set; }
        public DateTime payment_delivery_date { get; set; }
        public DateTime payment_cutoff_time { get; set; }
        public string payment_type { get; set; }
        public string currency { get; set; }
        public string bank_country { get; set; }
    }
}
