using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetDataPlanRequest
    {
        public long ProviderId { get; set; }
        public string CountryCode { get; set; } = null!;
    }
}
