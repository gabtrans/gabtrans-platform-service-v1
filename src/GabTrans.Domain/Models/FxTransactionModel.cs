using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FxTransactionModel
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionReference { get; set; }
        public string TransactionType { get; set; }
        public string AccountType { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public string Amount { get; set; }
        public string ConvertedAmount { get; set; }
        public string TransactionFee { get; set; }
        public string Status { get; set; }
    }
}
