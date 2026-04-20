using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CompleteSignUpRequest
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
