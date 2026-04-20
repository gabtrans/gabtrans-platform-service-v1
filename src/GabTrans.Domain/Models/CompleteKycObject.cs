using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CompleteKycObject
    {
        public long AccountId { get; set; }
        public long AccountTypeId { get; set; }
        public string?[] PurposeOfAccounts { get; set; }
        public string? PurposeOfAccount { get; set; }
        public long? OccupationId { get; set; }
        public string JobBrief { get; set; }
        public bool IsBusinessRegulated { get; set; }
        public string? IdentificationType { get; set; }
        public string? BusinessName { get; set; }
        public string? AccountName { get; set; }
        public string? EmailAddress { get; set; }
        public string? RegisteredAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? OperatingAddress { get; set; }
        public string? OperatingStreet { get; set; }
        public string? OperatingCity { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public long NatureOfBusinessId { get; set; }
        public long? RegistrationBodyId { get; set; }
        public string? BusinessNumber { get; set; }
        public string? TransactionValue { get; set; }
        public string? TransactionVolume { get; set; }
        public string? BusinessType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? DestinationOfFund { get; set; }
        public string[] DestinationOfFunds { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ResidentialAddress { get; set; }
        public string? ResidentialCity { get; set; }
    }
}
