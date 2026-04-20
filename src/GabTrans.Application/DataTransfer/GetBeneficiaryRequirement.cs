using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetBeneficiaryRequirement : BaseRequest
    {
        public string ReceiverCountryCode { get; set; }
        public long PaymentTypeId { get; set; }
        public long ReceiverEntityTypeId { get; set; }
    }
}
