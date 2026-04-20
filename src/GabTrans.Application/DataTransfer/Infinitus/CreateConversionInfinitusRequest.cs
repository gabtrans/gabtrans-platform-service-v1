using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateConversionInfinitusRequest
    {
        public string buyCurrency { get; set; }
        public string sellCurrency { get; set; }
        public double buyAmount { get; set; }
        public double sellAmount { get; set; }
    }
}
