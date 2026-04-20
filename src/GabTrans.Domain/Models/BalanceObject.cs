using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BalanceObject
    {
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
    }
}
