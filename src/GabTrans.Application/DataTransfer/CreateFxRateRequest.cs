using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CreateFxRateRequest
    {
        public required string From { get; set; }
        public required string To { get; set; }
        public required decimal Rate { get; set; }
        public required long? AccountId { get; set; }
        public required decimal RateFromProvider { get; set; }
        public required decimal RateMarkUp { get; set; }
    }
}
