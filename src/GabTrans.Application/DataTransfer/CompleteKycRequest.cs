using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class CompleteKycRequest : BaseRequest
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
        public string[] DestinationOfFunds { get; set; }
    }
}
