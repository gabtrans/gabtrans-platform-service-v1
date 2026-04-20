using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class GetInfinitusTransactionRequest
    {
        public string? currency { get; set; }
        public string? status { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? type { get; set; }
    }
}
