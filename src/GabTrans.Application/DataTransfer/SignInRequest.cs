using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GabTrans.Application.DataTransfer
{
	public class SignInRequest
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

