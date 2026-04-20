using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class BalanceEnquiryRequest : BaseRequest
    {
        public string Currency { get; set; } = null!;
    }
}
