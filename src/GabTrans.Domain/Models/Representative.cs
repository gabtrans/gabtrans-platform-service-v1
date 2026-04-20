using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class Representative
    {
        public long Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? EmailAddress { get; set; }

        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? ResidentialState { get; set; }

        public string? TaxNumber { get; set; }

        public string? Citizenship { get; set; }

        public string? IdentityNumber { get; set; }

        public string? IdentityType { get; set; }

        public DateTime? IdentityIssueDate { get; set; }

        public DateTime? IdentityExpiryDate { get; set; }

        public string? PostalCode { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? Selfie { get; set; }

        public string? Country { get; set; }

        public DateTime UpdatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public string? Occupation { get; set; }

        public string? IncomeRange { get; set; }

        public string? OccupationDescription { get; set; }

        public string? EmploymentStatus { get; set; }

        public string Status { get; set; } = null!;

        public long BusinessId { get; set; }

        public string? Industry { get; set; }

        public string? Employer { get; set; }

        public string? EmployerSate { get; set; }

        public string? EmployerCountry { get; set; }

        public string? IncomeSource { get; set; }

        public string? IncomeState { get; set; }

        public string? IncomeCountry { get; set; }

        public string? SourceOfFunds { get; set; }

        public string? WealthSource { get; set; }

        public string? WealthSourceDescription { get; set; }

        public string? AnnualIncome { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? BankStatement { get; set; }

        public string? IdentityDocument { get; set; }

        public string? TaxDocument { get; set; }

        //public string? Role { get; set; }

        //public string? Title { get; set; }

        //public string? OwnershipPercentage { get; set; }

        //public bool IsSigner { get; set; }

        public string? Uuid { get; set; }
    }
}
