using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class SettlementModel
    {
        public long WalletId { get; set; }
        public decimal TransactionFee { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public string DebitCreditIndicator { get; set; }
        public long AccountId { get; set; }
        public string Reference { get; set; }
        public string Note { get; set; }
    }
}
