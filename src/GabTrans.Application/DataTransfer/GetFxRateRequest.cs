using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetFxRateRequest
    {
        public string ToCurrency { get; set; }
        public string FromCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
    }
}
