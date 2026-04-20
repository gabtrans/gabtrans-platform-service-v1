using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class ExchangeRateModel
    {
        public long Id { get; set; }

        public decimal Rate { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal? FriendlyDisplayAmount { get; set; }

        public string? Type { get; set; }
        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public long? UpdatedBy { get; set; }

        public decimal RateFromProvider { get; set; }

        public decimal RateMarkUp { get; set; }
    }
}
