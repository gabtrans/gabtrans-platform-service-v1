using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class FxConversionRequest
    {
        public TradeFxRequest TradeFxRequest { get; set; }
        public FxRateLog FxRateLog { get; set; }
    }
}
