using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class CardDetails
    {
        public string Pin { get; set; }
        public string ExpiryDate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal PreviousBalance { get; set; }
        public string BillingAddress { get; set; }
        public string ZipCode { get; set; }
        public string Label { get; set; }
        public string CardTypeId { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long Id { get; set; }
        public string FullName { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string Cvv { get; set; } = null!;
        public long CurrencyId { get; set; }
        public string IsLocked { get; set; }
        public string IsBlocked { get; set; }
        public string ToTalFunded { get; set; }
        public string ToTalSpent { get; set; }
    }
}
