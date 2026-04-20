using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class VirtualAccountModel
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string AccountHolderName { get; set; } = null!;
        public string? AccountType { get; set; }
        public string? ReferenceCode { get; set; }
        public string? BankStreet1 { get; set; }
        public string? BankStreet2 { get; set; }
        public string? BankCity { get; set; }
        public string? BankState { get; set; }
        public string? BankPostalCode { get; set; }
        public string? SwiftCode { get; set; }
        public string? RoutingNumber { get; set; }
    }
}
