using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GabTrans.Domain.Models
{
    public class UserDetails
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string DateRegistered { get; set; }
        public string? LastLogin { get; set; }
        public long Id { get; set; }
    }
}
