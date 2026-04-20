using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ReceiveMoneyRequest : BaseRequest
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string EmailOrPhoneNo { get; set; }
        public long PaymentTypeId { get; set; }
    }
}
