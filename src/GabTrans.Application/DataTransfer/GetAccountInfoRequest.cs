using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetAccountInfoRequest : BaseRequest
    {
        public string UserName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
