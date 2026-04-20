using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransactionMetadata
    {
        public string Vendor { get; set; }
        public string IpAddress { get; set; }
        public string InitiatedAt { get; set; }
        public string StartedAt { get; set; }
    }
}
