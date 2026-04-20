using GabTrans.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusAccountResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string provider { get; set; }
        public string createdAt { get; set; }
        public InfinitusAccountPrimaryContact primaryContact { get; set; }
        public InfinitusAccountBusinessDetails businessDetails { get; set; }
    }

    public class InfinitusAccountBusinessDetails
    {
        public string businessName { get; set; }
        public string businessStructure { get; set; }
        public InfinitusAccountRegistrationAddress registrationAddress { get; set; }
        //public List<InfinitusAccountBusinessIdentifier> businessIdentifiers { get; set; }
    }

    public class InfinitusAccountBusinessIdentifier
    {
        public string countryCode { get; set; }
        public string number { get; set; }
        public string type { get; set; }
    }

    public class InfinitusAccountPrimaryContact
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
    }

    public class InfinitusAccountRegistrationAddress
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string countryCode { get; set; }
        public string postcode { get; set; }
        public string state { get; set; }
    }
}
