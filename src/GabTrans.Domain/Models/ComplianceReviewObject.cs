using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ComplianceReviewObject
    {
        public long Id { get; set; }
        public string AccountType { get; set; }
        public string Frequency { get; set; }
        public string RiskLevel { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public long AccountTypeId { get; set; }
        public long RiskLevelId { get; set; }
        public long FrequencyId { get; set; }
    }
}
