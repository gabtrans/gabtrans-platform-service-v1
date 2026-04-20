using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class FundCardRequest : BaseRequest
    {
        public long CardId { get; set; }
        public decimal Amount { get; set; }
    }
}
