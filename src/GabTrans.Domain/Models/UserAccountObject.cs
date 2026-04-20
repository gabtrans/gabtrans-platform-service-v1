using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class UserAccountObject
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? EmailAddress { get; set; }

        public string? Address { get; set; }

        public string AccountType { get; set; }

        public long AccountTypeId { get; set; }

        public string? PurposeOfAccount { get; set; }

        public string? TransactionValue { get; set; }

        public string? TransactionVolume { get; set; }

        public string AccountStatus { get; set; }

        public bool IsScreened { get; set; }
        public bool IsPrimary { get; set; }
        public string? DestinationOfFunds { get; set; }
        public string? AccountUuid { get; set; }
        public string? ContactUuid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long BusinessId { get; set; }
    }
}
