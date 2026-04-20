using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class QueryTransfer
    {
        public string? AccountName { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? Reference { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
    }
}
