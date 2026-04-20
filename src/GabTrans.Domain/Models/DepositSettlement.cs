using GabTrans.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class DepositSettlement
    {
        public long WalletId { get; set; }
        public Deposit Deposit { get; set; }
        public Account Account { get; set; }
    }
}
