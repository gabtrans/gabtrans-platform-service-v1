using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class UpdateAuthorizationPinRequest
    {
        public string CurrentPin { get; set; }
        public string Pin { get; set; }
        public string PinConfirmation { get; set; }
    }
}
