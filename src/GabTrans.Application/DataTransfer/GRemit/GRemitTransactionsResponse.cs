using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.GRemit
{
    public class GRemitTransactionsResponse
    {
        public GRemitTransactionsResult Result { get; set; }
    }

    public class GRemitTransactionsDetails
    {
        public List<GRemitTransaction> Transaction { get; set; }
        // public List<GRemitTransaction> Transactions { get; set; } = new List<GRemitTransaction>();
    }

    public class GRemitTransactionsReceiver
    {
        public string? ReceiverKey { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? CountryCode { get; set; }
        public string? StateKey { get; set; }
        public string? StateCode { get; set; }
        public string? CityName { get; set; }
        public string? ZipCode { get; set; }
        public string? LandlineNo { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? BeneficiaryID { get; set; }
        public string? AccountTypeKey { get; set; }
        public string? AccountNo { get; set; }
        public string? RoutingNumber { get; set; }
        public string? BeneficiaryBankCode { get; set; }
        public string? BeneficiaryBankName { get; set; }
        public string? BeneficiaryBranchName { get; set; }
        public string? BeneficiaryBranchRoutingNo { get; set; }
        public string? RelationshipWithSender { get; set; }
        public string? Nationality { get; set; }
        public string? PickupNetworkKey { get; set; }
        public string? PickupLocationKey { get; set; }
    }

    public class GRemitTransactionsRemittance
    {
        public string? RemittanceDate { get; set; }
        public string? DeliveryMethodKey { get; set; }
        public string? DeliveryMethodCode { get; set; }
        public string? SendingCurrency { get; set; }
        public string? SendingAmount { get; set; }
        public string? ServiceChargesAmount { get; set; }
        public string? ExchangeRate { get; set; }
        public string? ReceivingCurrency { get; set; }
        public string? ReceivingAmount { get; set; }
        public string? PurposeOfRemittance { get; set; }
        public string? MsgToPayee { get; set; }
        public string? MsgToBeneficiary { get; set; }
        public string? SecurityCode { get; set; }
        public string? PayeeReferenceNo { get; set; }
        public string? PayeeLocationKey { get; set; }
        public string? SourceOfIncome { get; set; }
    }

    public class GRemitTransactionsResult
    {
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public string RecordCount { get; set; }
        public GRemitTransactionsDetails Details { get; set; }
    }

    public class GRemitTransactionsSender
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? CountryCode { get; set; }
        public string? StateKey { get; set; }
        public string? StateCode { get; set; }
        public string? CityName { get; set; }
        public string? ZipCode { get; set; }
        public string? LandlineNo { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }
        public string? ID1Type { get; set; }
        public string? ID1Number { get; set; }
        public string? ID1IssueDate { get; set; }
        public string? ID1ExpiryDate { get; set; }
        public string? ID1IssueAuthority { get; set; }
        public string? ID2Type { get; set; }
        public string? ID2Number { get; set; }
        public string? ID2IssueDate { get; set; }
        public string? ID2ExpiryDate { get; set; }
        public string? ID2IssueAuthority { get; set; }
        public string? DateOfBirth { get; set; }
        public string? GenderKey { get; set; }
        public string? Occupation { get; set; }
        public string? Nationality { get; set; }
        public string? SenderKey { get; set; }
    }

    public class GRemitTransaction
    {
        public GRemitTransactionsSender Sender { get; set; }
        public GRemitTransactionsReceiver Receiver { get; set; }
        public GRemitTransactionsRemittance Remittance { get; set; }
        public string ReferenceNo { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
}
