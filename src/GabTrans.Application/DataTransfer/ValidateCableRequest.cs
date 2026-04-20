using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ValidateCableRequest
    {
        public string Provider { get; set; } = null!;
        public string SmartCardNumber { get; set; } = null!;
    }
}
