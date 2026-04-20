using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CustomerRecord
    {
        public UserDetails UserDetails { get; set; }
        public decimal LastTransactionAmount { get; set; }
        public List<TransactionDetails> TransactionDetails { get; set; }
    }
}
