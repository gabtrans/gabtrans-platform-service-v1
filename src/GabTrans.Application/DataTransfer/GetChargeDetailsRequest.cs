using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetChargeDetailsRequest : BaseRequest
    {
        public long Id { get; set; }
    }
}
