using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ConfirmPhoneNoRequest 
    {
        public string ResidentialAddress { get; set; }
        public string? ResidentialAddress2 { get; set; }
        public string ResidentialStreet { get; set; }
        public string ResidentialCity { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not matched")]
        public string PasswordConfirmation { get; set; }
        public long ChannelId { get; set; }
        public string IPAddress { get; set; }
        public string? DeviceId { get; set; }
        public string? Browser { get; set; }
        public long Actor { get; set; }
    }
}
