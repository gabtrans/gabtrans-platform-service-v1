using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetCardRequest : BaseRequest
    {
        public string? CurrencyCode { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public long? StatusId { get; set; }
        public bool Complete { get; set; }
    }
}
