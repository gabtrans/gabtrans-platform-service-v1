using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ProcessDeposit
    {
        public decimal Fee { get; set; }
        public long AccountId { get; set; }
        public PendingDeposit PendingDeposit { get; set; }
    }
}
