using GabTrans.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GabTrans.Application.DataTransfer
{
    public class CompleteOnboardingRequest : BaseRequest
    {
        public long AccountTypeId { get; set; }
        public string[] PurposeOfAccount { get; set; }
        public long OccupationId { get; set; }
        public string JobBrief { get; set; }
        public bool IsBusinessRegulated { get; set; }
        public string IdentificationType { get; set; }
        public string? BusinessName { get; set; }
        public string? AccountName { get; set; }
        public string? EmailAddress { get; set; }
        public string? RegisteredAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? IndividualBVN { get; set; }
        public string? OperatingAddress { get; set; }
        public string? OperatingStreet { get; set; }
        public string? OperatingCity { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public long NatureOfBusinessId { get; set; }
        public long? RegistrationBodyId { get; set; }
        public string? CompanyNumber { get; set; }
        public string? BusinessLicenseNumber { get; set; }
        public string? TransactionValue { get; set; }
        public string? TransactionVolume { get; set; }
        public string? BusinessType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string[]? DestinationOfFunds { get; set; }
        public string? FaceBook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Website { get; set; }
        public string? DirectorName { get; set; }
        public string? DirectorBVN { get; set; }
        public string? CountryCode { get; set; }
        public long? BusinessId { get; set; }
        public List<ShareHolder> ShareHolders { get; set; } = new List<ShareHolder>();
    }
}

