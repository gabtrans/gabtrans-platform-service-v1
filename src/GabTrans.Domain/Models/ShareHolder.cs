using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ShareHolder
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string? EmailAddress { get; set; }
        public string? ResidentialAddress { get; set; }
        public string? CountryOfResidence { get; set; }
        public decimal Percentage { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
