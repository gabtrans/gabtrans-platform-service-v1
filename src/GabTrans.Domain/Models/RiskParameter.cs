using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class RiskParameter
    {
        public long RiskFactorId { get; set; }
        public long WeightingFactor { get; set; }
        public long AccountType { get; set; }
    }
}
