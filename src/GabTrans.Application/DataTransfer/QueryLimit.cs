using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class QueryLimit
    {
        public long? AccountId { get; set; }
        //public string? StartDate { get; set; }
        //public string? EndDate { get; set; }
        public string? Currency { get; set; }
        public string? TransactionType { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}
