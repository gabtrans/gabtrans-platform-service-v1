using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.GRemit
{
    public class GRemitUpdateTransactionResponse
    {
        public GRemitUpdateResult Result { get; set; }
    }

    public class GRemitUpdateDetails
    {
        public GRemitUpdateTransaction Transaction { get; set; }
    }

    public class GRemitUpdateResult
    {
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public GRemitUpdateDetails Details { get; set; }
    }

    public class GRemitUpdateTransaction
    {
        public string ReferenceNo { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
    }
}
