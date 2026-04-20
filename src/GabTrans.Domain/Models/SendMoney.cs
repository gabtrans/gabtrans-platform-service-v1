using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class SendMoney
    {
        public long PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public bool AddAsBen { get; set; }
        public long? BeneficiaryId { get; set; }
        public string? BeneficiaryName { get; set; }
        public string Currency { get; set; }
        public string? BankCountry { get; set; }
        public string? Reason { get; set; }
        public string? BeneficiaryAddress { get; set; }
        public string? BeneficiaryCountry { get; set; }
        public string? AccountNumber { get; set; }
        public string? BicSwift { get; set; }
        public string? SortCode { get; set; }
        public string? Iban { get; set; }
        public string? BankAddress { get; set; }
        public string? BankName { get; set; }
        public long AccountKycType { get; set; }
        //public string? BeneficiaryEmail { get; set; }
        //public string? BankAccountType { get; set; }
        public long BeneficiaryEntityTypeId { get; set; }
        //public string? BeneficiaryCompanyName { get; set; }
        //public string? BeneficiaryFirstName { get; set; }
        //public string? BeneficiaryLastName { get; set; }
        public string? BeneficiaryCity { get; set; }
        public string? BeneficiaryPostcode { get; set; }
        public string? BeneficiaryStateOrProvince { get; set; }
        public string? BeneficiaryDateOfBirth { get; set; }
        //public string? BeneficiaryIdentificationType { get; set; }
        //public string? BeneficiaryIdentificationValue { get; set; }
       // public string? Purpose { get; set; }
        //public string PayerName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        //public string IdentificationNumber { get; set; } = null!;
        //public long? IdentificationTypeId { get; set; }
        public string FullName { get; set; } = null!;
        public long UserId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? BankCode { get; set; }
        public long AccountId { get; set; }
        //public string ContactUuid { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountName { get; set; } = null!;
        public string BeneficiaryCurrency { get; set; } 
        public long Channel { get; set; }
    }
}
