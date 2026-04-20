using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CreateConversionRequest : BaseRequest
    {
        public string BuyCurrency { get; set; }
        public string SellCurrency { get; set; }
        public string FixedSide { get; set; }
        public string Amount { get; set; }
        public string Reason { get; set; }
    }
}
