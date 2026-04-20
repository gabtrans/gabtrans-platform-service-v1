using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.GRemit
{
    public class GRemitTransactionResponse
    {
        public GRemitTransactionResult Result { get; set; }
    }

    public class GRemitTransactionDetails
    {
        public GRemitTransaction Transaction { get; set; }
    }

    public class GRemitTransactionResult
    {
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public string RecordCount { get; set; }
        public GRemitTransactionDetails Details { get; set; }
    }
}
