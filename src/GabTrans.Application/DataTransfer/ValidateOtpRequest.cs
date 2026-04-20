using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ValidateOtpRequest : BaseRequest
    {
        public string OTP { get; set; }
    }
}
