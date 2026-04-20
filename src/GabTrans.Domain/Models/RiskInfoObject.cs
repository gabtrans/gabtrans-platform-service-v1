using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class RiskInfoObject
    {
        public string RiskFactor { get; set; }
        public string Value { get; set; }
        public long WeightingFactor { get; set; }
        public long Score { get; set; }
        public string Status { get; set; }
        public decimal WeightedScore { get; set; }
    }
}
