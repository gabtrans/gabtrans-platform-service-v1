using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class Screening
    {
        public string Status { get; set; }
        public string SearchDate { get; set; }
        public string SearchReference { get; set; }
        public string Vendor { get; set; }
        public int CountOfResult { get; set; }
        public string SearchId { get; set; }
        public string SearchResultUrl { get; set; }
    }
}
