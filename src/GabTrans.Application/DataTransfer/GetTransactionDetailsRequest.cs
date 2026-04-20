using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetTransactionDetailsRequest : BaseRequest
    {
        public long? TransactionId { get; set; }
        public string ReferenceNumber { get; set; }
    }
}
