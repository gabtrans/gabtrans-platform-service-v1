using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BusinessInformationModel
    {
        public string? Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
        public DateTime IncorporationDate { get; set; }
        public string TradeName { get; set; }
        public string Website { get; set; }
        public string MonthlyRevenue { get; set; }
        public string MonthlyConversionVolumeDigitalAssets { get; set; }
        public string MonthlyLocalPaymentVolume { get; set; }
        public string MonthlyConversionVolumeFiat { get; set; }
        public string MonthlySWIFTVolume { get; set; }
        public string CurrenciesNeeded { get; set; }
        public string CountriesOfOperation { get; set; }
        public string? MainIndustry { get; set; }
        public string? NAICS { get; set; }
        public string? NAICSDescription { get; set; }
    }
}
