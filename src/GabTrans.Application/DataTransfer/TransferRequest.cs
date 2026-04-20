using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class TransferRequest
    {
        public decimal Amount { get; set; }
        //public long? RecipientId { get; set; }
        public string TransactionPin { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string? Name { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? CountryCode { get; set; }
        public string? AddressPostCode { get; set; }
        public string? AddressStreetAddress { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? BankCountry { get; set; }
        public string? Currency { get; set; }
        public string? BankAccountType { get; set; }
        public string? SwiftCode { get; set; }
        public string? RoutingNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? AccountRoutingType { get; set; }
        public string? BankBranch { get; set; }
        public string? BankName { get; set; }
        public string? Iban { get; set; }
        public string? BankStreetAddress { get; set; }
        public string? BankCity { get; set; }
        public string? BankPostalCode { get; set; }
        public string? BankState { get; set; }
        public string? IntermediaryBankCountry { get; set; }
        public string? IntermediaryPostalCode { get; set; }
        public string? IntermediaryCity { get; set; }
        public string? IntermediaryState { get; set; }
        public string? IntermediaryStreet1 { get; set; }
        public string? IntermediaryStreet2 { get; set; }
        public string? IntermediaryBankName { get; set; }
        public string? IntermediaryRoutingCode { get; set; }
        public string? InternationalBankName { get; set; }
    }
}
