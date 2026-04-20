using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class WalletHistory
    {
        public long Id { get; set; }

        public string Currency { get; set; }

        public string Amount { get; set; }

        public string Fee { get; set; }

        public string Type { get; set; }

        public string ReferenceNumber { get; set; }

        public string Beneficiary { get; set; }

        public string Sender { get; set; }

        public string CreatedAt { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public bool Buderless { get; set; }
    }
}
