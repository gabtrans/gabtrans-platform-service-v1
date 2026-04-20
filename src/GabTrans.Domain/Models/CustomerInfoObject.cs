using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CustomerInfoObject
    {
        public string Selfie { get; set; }
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public string CustomerNumber { get; set; }
        public string OnboardedOn { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Flag { get; set; }
        public string KycReviewDate { get; set; }
        public decimal RiskScore { get; set; }
        public long AvgRiskScore { get; set; }
        public string AccountType { get; set; }
        public string BusinessType { get; set; }
        public string CompanyNumber { get; set; }
        public string BusinessName { get; set; }
        public string BaseCurrency { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set;}
        public string CountryOfResidence { get; set; }
        public string PersonalAddress { get; set; }
        public string Occupation { get; set; }
        public string KYCVerified { get; set; }
        public string AddressVerified { get; set; }
        public string AddressVerification { get; set; }
        public string KYCVerificationDate { get; set; }
        public string KYCOutcome { get; set; }
        public string RiskCategory { get; set; }
    }
}
