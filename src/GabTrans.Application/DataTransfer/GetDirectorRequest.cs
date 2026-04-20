using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetDirectorRequest : BaseRequest
    {
        public long? BusinessId { get; set; }
        public string? CompanyNumber { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
