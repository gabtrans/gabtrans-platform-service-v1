using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CreateUserRequest
    {
        public string Role { get; set; }
        public string EmailAddress { get; set; }
    }
}
