using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ValidateElectricityRequest
    {
        public string Provider { get; set; } = null!;
        public string BillingType { get; set; } = null!;
        public string MeterNumber { get; set; } = null!;
    }
}
