using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class AccountCurrencyObject
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public decimal BookBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal LienAmount { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string Flag { get; set; }
    }
}
