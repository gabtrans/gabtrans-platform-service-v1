using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class SendSmsRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string CountryCode { get; set; }
    }
}
