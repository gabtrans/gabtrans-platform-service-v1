using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class OnboardingObject
    {
        public long UserId { get; set; }
        public bool PhoneVerified { get; set; }
        public bool EmailVerified { get; set; }
        public string? OTP { get; set; }
        public string? Token { get; set; }
        public bool Completed { get; set; }
        public bool Verified { get; set; }
        public bool HasPin { get; set; }
        public string? Document { get; set; }
        public DateTime? CustomDOB { get; set; }
    }
}
