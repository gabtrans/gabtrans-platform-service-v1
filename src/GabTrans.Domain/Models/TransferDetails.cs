using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransferDetails
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string AccountName { get; set; }

        public string EmailAddress { get; set; }

        public string Reference { get; set; } = null!;

        public string SourceCurrency { get; set; } = null!;

        public string Amount { get; set; }

        public decimal Fee { get; set; }

        public string Status { get; set; } = null!;

        public string ProcessingStatus { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public string Reason { get; set; } = null!;

        public string? Comment { get; set; }

        public string? FailureReason { get; set; }

        public string? PostBackResponse { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public string Gateway { get; set; } = null!;

        public string? GatewayReference { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public decimal AmountPaid { get; set; }

        public string Type { get; set; } = null!;

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Country { get; set; }

        public string? PostCode { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? BankCountry { get; set; }

        public string? BankAccountType { get; set; }

        public string? SwiftCode { get; set; }

        public string? RoutingNumber { get; set; }

        public string? AccountNumber { get; set; }

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

        public string? Uuid { get; set; }
    }
}
