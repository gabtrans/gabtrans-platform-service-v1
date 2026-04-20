using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetKycApprovalRequest
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
        public long? Id { get; set; }
    }
}
