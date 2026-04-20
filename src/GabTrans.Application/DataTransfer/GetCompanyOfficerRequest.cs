using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCompanyOfficerRequest : BaseRequest
    {
        public string Appointmnent { get; set; }
        public string CompanyNumber { get; set; }
    }
}
