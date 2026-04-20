using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetTransactionHistoryRequest
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Category { get; set; }
        public long AccountId { get; set; }
    }
}
