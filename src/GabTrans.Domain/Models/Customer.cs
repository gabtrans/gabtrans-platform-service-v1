using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string DateCreated { get; set; }
        public string DateOfBirth { get; set; }
    }
}
