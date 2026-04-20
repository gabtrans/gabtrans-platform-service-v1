using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GabTrans.Application.DataTransfer
{
    public class ConfirmEmailAddressRequest
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public long ChannelId { get; set; }
        public string IPAddress { get; set; }
        public string? DeviceId { get; set; }
        public string? Browser { get; set; }
        public string CountryCode { get; set; }
    }
}
