using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class PinRequest : BaseRequest
    {
        public string Pin { get; set; }
        public string? OldPin { get; set; }
        public string PinConfirmation { get; set; }
    }
}
