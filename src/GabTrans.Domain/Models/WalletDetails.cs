using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class WalletDetails
    {
        public string AccountName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public string DateCreated { get; set; }
    }
}
