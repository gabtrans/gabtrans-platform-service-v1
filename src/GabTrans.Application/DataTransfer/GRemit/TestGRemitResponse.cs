using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GabTrans.Application.DataTransfer.GRemit
{
    //internal class TestGRemitResponse
    //{
    //}
    [XmlRoot(ElementName = "Sender")]
    public class Sender
    {

        [XmlElement(ElementName = "FirstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "MiddleName")]
        public object MiddleName { get; set; }

        [XmlElement(ElementName = "LastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "AddressLine1")]
        public string AddressLine1 { get; set; }

        [XmlElement(ElementName = "AddressLine2")]
        public object AddressLine2 { get; set; }

        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }

        [XmlElement(ElementName = "StateKey")]
        public int StateKey { get; set; }

        [XmlElement(ElementName = "StateCode")]
        public string StateCode { get; set; }

        [XmlElement(ElementName = "CityName")]
        public string CityName { get; set; }

        [XmlElement(ElementName = "ZipCode")]
        public string ZipCode { get; set; }

        [XmlElement(ElementName = "LandlineNo")]
        public object LandlineNo { get; set; }

        [XmlElement(ElementName = "MobileNo")]
        public double MobileNo { get; set; }

        [XmlElement(ElementName = "EmailAddress")]
        public string EmailAddress { get; set; }

        [XmlElement(ElementName = "ID1Type")]
        public string ID1Type { get; set; }

        [XmlElement(ElementName = "ID1Number")]
        public string ID1Number { get; set; }

        [XmlElement(ElementName = "ID1IssueDate")]
        public DateTime ID1IssueDate { get; set; }

        [XmlElement(ElementName = "ID1ExpiryDate")]
        public DateTime ID1ExpiryDate { get; set; }

        [XmlElement(ElementName = "ID1IssueAuthority")]
        public object ID1IssueAuthority { get; set; }

        [XmlElement(ElementName = "ID2Type")]
        public string ID2Type { get; set; }

        [XmlElement(ElementName = "ID2Number")]
        public int ID2Number { get; set; }

        [XmlElement(ElementName = "ID2IssueDate")]
        public object ID2IssueDate { get; set; }

        [XmlElement(ElementName = "ID2ExpiryDate")]
        public object ID2ExpiryDate { get; set; }

        [XmlElement(ElementName = "ID2IssueAuthority")]
        public object ID2IssueAuthority { get; set; }

        [XmlElement(ElementName = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [XmlElement(ElementName = "GenderKey")]
        public int GenderKey { get; set; }

        [XmlElement(ElementName = "Occupation")]
        public object Occupation { get; set; }

        [XmlElement(ElementName = "Nationality")]
        public object Nationality { get; set; }

        [XmlElement(ElementName = "SenderKey")]
        public object SenderKey { get; set; }
    }

    [XmlRoot(ElementName = "Receiver")]
    public class Receiver
    {

        [XmlElement(ElementName = "ReceiverKey")]
        public object ReceiverKey { get; set; }

        [XmlElement(ElementName = "FirstName")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "MiddleName")]
        public object MiddleName { get; set; }

        [XmlElement(ElementName = "LastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "AddressLine1")]
        public string AddressLine1 { get; set; }

        [XmlElement(ElementName = "AddressLine2")]
        public object AddressLine2 { get; set; }

        [XmlElement(ElementName = "CountryCode")]
        public string CountryCode { get; set; }

        [XmlElement(ElementName = "StateKey")]
        public int StateKey { get; set; }

        [XmlElement(ElementName = "StateCode")]
        public object StateCode { get; set; }

        [XmlElement(ElementName = "CityName")]
        public string CityName { get; set; }

        [XmlElement(ElementName = "ZipCode")]
        public int ZipCode { get; set; }

        [XmlElement(ElementName = "LandlineNo")]
        public object LandlineNo { get; set; }

        [XmlElement(ElementName = "MobileNo")]
        public int MobileNo { get; set; }

        [XmlElement(ElementName = "EmailAddress")]
        public object EmailAddress { get; set; }

        [XmlElement(ElementName = "BeneficiaryID")]
        public object BeneficiaryID { get; set; }

        [XmlElement(ElementName = "AccountTypeKey")]
        public int AccountTypeKey { get; set; }

        [XmlElement(ElementName = "AccountNo")]
        public int AccountNo { get; set; }

        [XmlElement(ElementName = "RoutingNumber")]
        public object RoutingNumber { get; set; }

        [XmlElement(ElementName = "BeneficiaryBankCode")]
        public int BeneficiaryBankCode { get; set; }

        [XmlElement(ElementName = "BeneficiaryBankName")]
        public string BeneficiaryBankName { get; set; }

        [XmlElement(ElementName = "BeneficiaryBranchName")]
        public object BeneficiaryBranchName { get; set; }

        [XmlElement(ElementName = "BeneficiaryBranchRoutingNo")]
        public object BeneficiaryBranchRoutingNo { get; set; }

        [XmlElement(ElementName = "RelationshipWithSender")]
        public string RelationshipWithSender { get; set; }

        [XmlElement(ElementName = "Nationality")]
        public object Nationality { get; set; }

        [XmlElement(ElementName = "PickupNetworkKey")]
        public int PickupNetworkKey { get; set; }

        [XmlElement(ElementName = "PickupLocationKey")]
        public int PickupLocationKey { get; set; }
    }

    [XmlRoot(ElementName = "Remittance")]
    public class Remittance
    {

        [XmlElement(ElementName = "RemittanceDate")]
        public DateTime RemittanceDate { get; set; }

        [XmlElement(ElementName = "DeliveryMethodKey")]
        public int DeliveryMethodKey { get; set; }

        [XmlElement(ElementName = "DeliveryMethodCode")]
        public string DeliveryMethodCode { get; set; }

        [XmlElement(ElementName = "SendingCurrency")]
        public string SendingCurrency { get; set; }

        [XmlElement(ElementName = "SendingAmount")]
        public DateTime SendingAmount { get; set; }

        [XmlElement(ElementName = "ServiceChargesAmount")]
        public int ServiceChargesAmount { get; set; }

        [XmlElement(ElementName = "ExchangeRate")]
        public int ExchangeRate { get; set; }

        [XmlElement(ElementName = "ReceivingCurrency")]
        public string ReceivingCurrency { get; set; }

        [XmlElement(ElementName = "ReceivingAmount")]
        public int ReceivingAmount { get; set; }

        [XmlElement(ElementName = "PurposeOfRemittance")]
        public string PurposeOfRemittance { get; set; }

        [XmlElement(ElementName = "MsgToPayee")]
        public object MsgToPayee { get; set; }

        [XmlElement(ElementName = "MsgToBeneficiary")]
        public object MsgToBeneficiary { get; set; }

        [XmlElement(ElementName = "SecurityCode")]
        public object SecurityCode { get; set; }

        [XmlElement(ElementName = "PayeeReferenceNo")]
        public object PayeeReferenceNo { get; set; }

        [XmlElement(ElementName = "PayeeLocationKey")]
        public int PayeeLocationKey { get; set; }

        [XmlElement(ElementName = "SourceOfIncome")]
        public object SourceOfIncome { get; set; }
    }

    [XmlRoot(ElementName = "Transaction")]
    public class Transaction
    {

        [XmlElement(ElementName = "Sender")]
        public Sender Sender { get; set; }

        [XmlElement(ElementName = "Receiver")]
        public Receiver Receiver { get; set; }

        [XmlElement(ElementName = "Remittance")]
        public Remittance Remittance { get; set; }

        [XmlElement(ElementName = "ReferenceNo")]
        public string ReferenceNo { get; set; }

        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        [XmlElement(ElementName = "StatusCode")]
        public int StatusCode { get; set; }
    }

    [XmlRoot(ElementName = "Details")]
    public class Details
    {

        [XmlElement(ElementName = "Transaction")]
        public Transaction Transaction { get; set; }
    }

    [XmlRoot(ElementName = "Result")]
    public class Result
    {

        [XmlElement(ElementName = "ResultCode")]
        public int ResultCode { get; set; }

        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        [XmlElement(ElementName = "RecordCount")]
        public int RecordCount { get; set; }

        [XmlElement(ElementName = "Details")]
        public Details Details { get; set; }
    }

}
