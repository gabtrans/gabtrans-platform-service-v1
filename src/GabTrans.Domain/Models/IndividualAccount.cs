using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class IndividualAccount
    {
        public string JumioRiskScore { get; set; }
        public string AmlComplianceScore { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; } = null!;
        public string? DateOfBirth { get; set; }
        public string CountryOfResidence { get; set; } = null!;
        public string? PersonalAddress { get; set; }
        public string? Occupation { get; set; }
        public string? PurposeOfAccount { get; set; }
        public string? KycVerified { get; set; }
        public string? KycVerificationDate { get; set; }
        public string? KycOutcome { get; set; }
        public string? RiskCategory { get; set; }
        public string? DateOnboarded { get; set; }
        public string? NextAssessmentDate { get; set; }
        public long AccountId { get; set; }
        public List<BalanceObject> TotalBalance { get; set; }
        public List<CardDetails> VirtualCards { get; set; }
        public List<CardDetails> PhysicalCards { get; set; }
        public List<TransactionObject> Transactions { get; set; }
    }
}
