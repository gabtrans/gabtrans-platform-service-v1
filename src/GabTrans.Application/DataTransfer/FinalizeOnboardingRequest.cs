using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class FinalizeOnboardingRequest
    {
        public string Password { get; set; } = null!;
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not matched")]
        public string PasswordConfirmation { get; set; } = null!;
        public long ChannelId { get; set; }
        public string IpAddress { get; set; }
        public string? DeviceId { get; set; }
        public string? Browser { get; set; }
    }
}
