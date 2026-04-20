using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateInfinitoGlobalAccountRequest
    {
        public string currency { get; set; }
        public string countryCode { get; set; }
        public string nickName { get; set; }
    }
}
