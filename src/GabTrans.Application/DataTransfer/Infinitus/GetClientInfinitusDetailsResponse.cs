using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class GetClientInfinitusDetailsResponse
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string contactEmail { get; set; }
        public string contactFirstName { get; set; }
        public string contactLastName { get; set; }
        public InfinitusOnboardingDetails onboardingDetails { get; set; }
    }

    public class InfinitusAddress
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
    }

    public class InfinitusIdDocument
    {
        public string number { get; set; }
        public string type { get; set; }
        public string issueDate { get; set; }
        public string expirationDate { get; set; }
    }

    public class InfinitusOnboardingDetails
    {
        public List<InfinitusPerson> persons { get; set; }
    }

    public class InfinitusPerson
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string dateOfBirth { get; set; }
        public string citizenship { get; set; }
        public string taxNumber { get; set; }
        public InfinitusAddress address { get; set; }
        public InfinitusIdDocument idDocument { get; set; }
        public string employer { get; set; }
        public string employerState { get; set; }
        public string employerCountry { get; set; }
        public string employmentStatus { get; set; }
        public string industry { get; set; }
        public string occupation { get; set; }
        public string occupationDescription { get; set; }
        public string incomeSource { get; set; }
        public string incomeState { get; set; }
        public string incomeCountry { get; set; }
        public string sourceOfFunds { get; set; }
        public string wealthSource { get; set; }
        public string wealthSourceDescription { get; set; }
        public string annualIncome { get; set; }
    }
}
