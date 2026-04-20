using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateInfinitusRecipientRequest
    {
        public string recipientAccountType { get; set; }
        public string paymentMethod { get; set; }
        public string? companyName { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public InfinitusRecipientAdditionalInfo additionalInfo { get; set; }
        public InfinitusRecipientAddress address { get; set; }
        public InfinitusRecipientBankDetails bankDetails { get; set; }
        public InfinitusRecipientIntermediaryBankAddress intermediaryBankAddress { get; set; }
        public string intermediaryBankName { get; set; }
        public string intermediaryRoutingCode { get; set; }
        public string internationalBankName { get; set; }
    }

    public class InfinitusRecipientAdditionalInfo
    {
        public string email { get; set; }
        public string businessPhoneNumber { get; set; }
        public string dateOfBirth { get; set; }
        public string personalMobileNumber { get; set; }
    }

    public class InfinitusRecipientAddress
    {
        public string countryCode { get; set; }
        public string state { get; set; }
        public string? postCode { get; set; }
        public string? streetAddress { get; set; }
        public string? city { get; set; }
    }

    public class InfinitusRecipientBankDetails
    {
        public string bankCountryCode { get; set; }
        public string accountCurrency { get; set; }
        public string bankAccountType { get; set; }
        public string swiftCode { get; set; }
        public string? routingNumber { get; set; }
        public string? accountNumber { get; set; }
        public string? accountName { get; set; }
        public string? accountRoutingType1 { get; set; }
        public string? bankBranch { get; set; }
        public string? bankName { get; set; }
        [JsonProperty("IBAN")]
        public string? iban { get; set; }
        public string? bankStreetAddress { get; set; }
        public string? bankCity { get; set; }
        public string? bankPostalCode { get; set; }
        public string? bankState { get; set; }

    }

    public class InfinitusRecipientIntermediaryBankAddress
    {
        public string country { get; set; }
        public string state { get; set; }
        public string? postalCode { get; set; }
        public string? street1 { get; set; }
        public string? street2 { get; set; }
        public string? city { get; set; }
    }
}
