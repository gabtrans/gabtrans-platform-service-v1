using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class Officers
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Premises { get; set; }
        public string CountryOfResidence { get; set; }
        public string DateOfBirth { get; set; }
        public string Occupation { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string Locality { get; set; }
        public string Nationality { get; set; }
        public string AppointedOn { get; set; }
        public string Role { get; set; }
        public string Appointment { get; set; }
    }
}
