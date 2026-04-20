using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class RepresentativeInfinitusResponse
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string dateOfBirth { get; set; }
        public string citizenship { get; set; }
        public string taxNumber { get; set; }
        public InfinitusIdDocument idDocument { get; set; }
        public InfinitusAddress address { get; set; }
        public string employmentStatus { get; set; }
        public string industry { get; set; }
        public string occupation { get; set; }
        public string occupationDescription { get; set; }
        public string employer { get; set; }
        public string employerState { get; set; }
        public string employerCountry { get; set; }
        public string incomeSource { get; set; }
        public string incomeState { get; set; }
        public string incomeCountry { get; set; }
        public string sourceOfFunds { get; set; }
        public string wealthSource { get; set; }
        public string wealthSourceDescription { get; set; }
        public string annualIncome { get; set; }
        public string role { get; set; }
        public string title { get; set; }
        public string ownershipPercentage { get; set; }
        public bool isSigner { get; set; }
    }
}
