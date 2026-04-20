using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ResetAccountPasswordRequest
    {
        public string Otp { get; set; }
    }
}
