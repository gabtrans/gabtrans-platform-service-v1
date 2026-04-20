using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FxRateModel
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public decimal ToAmount { get; set; }
        public decimal FromAmount { get; set; }
        public string RateToken { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime RateExpiry { get; set; }
        public int RateValidityInSeconds { get; set; }
    }
}
