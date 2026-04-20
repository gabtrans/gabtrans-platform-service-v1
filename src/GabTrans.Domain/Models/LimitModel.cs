using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class LimitModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string AccountName { get; set; }

        public decimal? SingleCumulative { get; set; }

        public decimal? DailyCumulative { get; set; }

        public long? DailyCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? TransactionType { get; set; }

        public string? AccountType { get; set; }

        public string Currency { get; set; } = null!;
    }
}
