using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ConversionDetails
    {
        public string Id { get; set; }
        public DateTime SettlementDate { get; set; }
        public DateTime ConversionDate { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SellAmount { get; set; }
        public string ShortReference { get; set; }
        public string CurrencyPair { get; set; }
        public string Status { get; set; }
        public string BuyCurrency { get; set; }
        public string SellCurrency { get; set; }
        public string FixedSide { get; set; }
        public float CoreRate { get; set; }
        public float PartnerRate { get; set; }
        public float Rate { get; set; }
        public decimal UnallocatedFunds { get; set; }
        public float MidMarketRate { get; set; }
    }
}
