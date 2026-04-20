using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class ConversionInfinitusResponse
    {
        public string id { get; set; }
        public string buyCurrency { get; set; }
        public string sellCurrency { get; set; }
        public double buyAmount { get; set; }
        public double sellAmount { get; set; }
        public double clientRate { get; set; }
        public string conversionDate { get; set; }
        public string shortReferenceId { get; set; }
        public string status { get; set; }
    }
}
