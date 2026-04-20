using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class RateDetails
    {
        public DateTime CutOffTime { get; set; }
        public string CurrencyPair { get; set; }
        public string BuyCurrency { get; set; }
        public string SellCurrency { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SellAmount { get; set; }
        public string FixedSide { get; set; }
        public float Rate { get; set; }
        public float PartnerRate { get; set; }
        public float CoreRate { get; set; }
        public decimal DepositAmount { get; set; }
        public string DepositCurrency { get; set; }
        public float MidMarketRate { get; set; }
    }
}
