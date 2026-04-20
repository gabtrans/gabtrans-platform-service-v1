using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusWalletBalanceResponse
    {
        public string accountId { get; set; }
        public string currency { get; set; }
        public string network { get; set; }
        public double availableAmount { get; set; }
        public double totalAmount { get; set; }
    }
}
