using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CustomerObject
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string DateJoined { get; set; }
        public long UserId { get; set; }
    }
}
