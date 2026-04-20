using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetPaymentPurpose : BaseRequest
    {
        public string Currency { get; set; }
        public string EntityType { get; set; }
        public string BankAccountCountry { get; set; }
    }
}
