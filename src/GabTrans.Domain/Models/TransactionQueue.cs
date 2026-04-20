using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransactionQueue
    {
        public long AccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? AccountNumber { get; set; }
    }
}
