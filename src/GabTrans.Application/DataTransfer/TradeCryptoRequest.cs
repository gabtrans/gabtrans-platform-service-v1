using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class TradeCryptoRequest
    {
        public string TransactionPin { get; set; }
        public string Network { get; set; }
        public string Asset { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
    }
}
