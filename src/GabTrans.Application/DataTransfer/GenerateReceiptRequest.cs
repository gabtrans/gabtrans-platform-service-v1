using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GenerateReceiptRequest : BaseRequest
    {
        public string? ReferenceNumber { get; set; }
        public long TransactionId { get; set; }
    }
}
