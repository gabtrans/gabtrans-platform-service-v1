using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BeneficiaryObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string BankCode { get; set; } = null!;
        public string Logo { get; set; }
        public string AccountHolder { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Currency { get; set; }
        public bool Internal { get; set; }
    }
}
