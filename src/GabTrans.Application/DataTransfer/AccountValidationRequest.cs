using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class AccountValidationRequest : BaseRequest
    {
        public bool Internal { get; set; }
        public string BankCode { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
    }
}
