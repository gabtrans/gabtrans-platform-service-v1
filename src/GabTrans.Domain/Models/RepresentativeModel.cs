using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class RepresentativeModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryOfResidence { get; set; }
        public string CountryCode { get; set; } = null!;
        public string? Occupation { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public long BusinessId { get; set; }
        public bool IsPushed { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Business { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsPep { get; set; }
        public bool HasAdverseMedia { get; set; }
        public bool IsScreened { get; set; }
        public bool IsVerified { get; set; }
        public string? IdentificationFront { get; set; }
        public string? IdentificationBack { get; set; }
        public string? SelfieFront { get; set; }
        public decimal? RiskScore { get; set; }
        public string? Outcome { get; set; }
        public string? Verification { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? Role { get; set; }
    }
}
