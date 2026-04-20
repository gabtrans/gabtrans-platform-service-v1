using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateTransferRequest
    {
        public string Otp { get; set; }
        public string Status { get; set; }
        public string? Comment { get; set; }
    }
}
