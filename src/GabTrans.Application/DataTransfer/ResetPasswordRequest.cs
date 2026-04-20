using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class ResetPasswordRequest
    {
        [Required]
        public string OTP { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password cannot be less than 6 characters")]
        [Required(ErrorMessage = "Enter new password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password not matched")]
        public string ConfirmPassword { get; set; }
    }
}
