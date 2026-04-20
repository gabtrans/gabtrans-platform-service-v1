using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CompanyBeneficialOwner
    {
        public string Name { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public string Kind { get; set; }
        public long BirthYear { get; set; }
        public string DateOfBirth { get; set; }
        public List<string> NatureOfControls { get; set; }
        public bool IsSanctioned { get; set; }
        public string CountryOfResidence { get; set; }
        public string AddressLine1 { get; set; }
        public string BeneficialId { get; set; }
        public string Locality { get; set; }
        public string Nationality { get; set; }
        public string Region { get; set; }
        public string Premises { get; set; }
        public decimal Percentage { get; set; }
    }
}
