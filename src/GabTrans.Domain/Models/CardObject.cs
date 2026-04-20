using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CardObject
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string DateCreated { get; set; }
        public string Status { get; set; }
    }
}
