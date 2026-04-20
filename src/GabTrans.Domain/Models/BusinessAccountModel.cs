using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BusinessAccountModel
    {
        public string Name { get; set; } = null!;

        public string OperatingAddress { get; set; } = null!;

        public string? RegisteredAddress { get; set; }

        public string? DateOfIncorporation { get; set; }

        public string? LicenseNumber { get; set; }

        public string? DocumentFront { get; set; }

        public string? DocumentBack { get; set; }

        public bool IsActive { get; set; }

        public long BusinessSectorId { get; set; }

        public string? BusinessSector { get; set; }

        public long? RegistrationBodyId { get; set; }

        public string? Introduction { get; set; }

        public string? BusinessType { get; set; }

        public string? OperatingStreet { get; set; }

        public string? OperatingCity { get; set; }

        public string? PostalCode { get; set; }

        public string? CountryOfRegistration { get; set; }

        public string? IsBusinessRegulated { get; set; }

        public string? PurposeOfAccount { get; set; }

        public string? KycVerified { get; set; }

        public string? KycVerificationDate { get; set; }
        public string? KycOutcome { get; set; }
        public string? RiskCategory { get; set; }
        public string? DateOnboarded { get; set; }
        public string? AmlComplianceScore { get; set; }
        public long AccountId { get; set; }
        public string? NextAssessmentDate { get; set; }
        public List<BalanceObject> TotalBalance { get; set; }
        public List<CardDetails> VirtualCards { get; set; }
        public List<CardDetails> PhysicalCards { get; set; }
        public List<RepresentativeModel> Directors { get; set; }
        public List<TransactionObject> Transactions { get; set; }
    }
}
