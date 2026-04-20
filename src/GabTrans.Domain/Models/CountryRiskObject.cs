using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CountryRiskObject
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long Weight { get; set; }
        public long RiskFactorId { get; set; }
        public string Country { get; set; }
        public string AccountType { get; set; }
    }
}
