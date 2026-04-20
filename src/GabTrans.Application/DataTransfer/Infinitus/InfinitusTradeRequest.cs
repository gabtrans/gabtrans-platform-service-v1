using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusTradeRequest
    {
        public string accountId { get; set; }
        public InfinitusTradeFrom from { get; set; }
        public InfinitusTradeTo to { get; set; }
    }
    public class InfinitusTradeFrom
    {
        public double amount { get; set; }
        public string network { get; set; }
        public string asset { get; set; }
    }

    public class InfinitusTradeTo
    {
        public string network { get; set; }
        public string asset { get; set; }
    }
}
