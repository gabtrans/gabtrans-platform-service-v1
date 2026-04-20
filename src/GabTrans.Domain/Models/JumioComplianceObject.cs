using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class JumioComplianceObject
    {
        public decimal RiskScore { get; set; }
        public IdentityVerification IdentityVerification { get; set; }
        public SelfCredentials SelfCredentials { get; set; }
        public FaceMap FaceMap { get; set; }
        public VerifiedDocument VerifiedDocument { get; set; }
        public Screening Screening { get; set; }
        public TransactionMetadata TransactionMetadata { get; set; }
    }
}
