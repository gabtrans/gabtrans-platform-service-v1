using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetVirtualAccountRequest : BaseRequest
    {
        public string BankCode { get; set; }
    }
}
