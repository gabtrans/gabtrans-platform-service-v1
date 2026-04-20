using System;
namespace GabTrans.Domain.Models
{
    public class AccountLimitDetails
    {
        public long Id { get; set; }

        public string AccountType { get; set; }

        public decimal DailyCumulative { get; set; }

        public long AccountId { get; set; }

        public decimal SingleCumulative { get; set; }

        public decimal TotalCumulative { get; set; }

        public string Currency { get; set; } = null!;

        public string? Type { get; set; }
    }
}

