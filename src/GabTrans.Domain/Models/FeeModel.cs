using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class FeeModel
    {
        public long Id { get; set; }

        public string TransactionType { get; set; } = null!;

        public decimal Rate { get; set; }

        public string Currency { get; set; } = null!;

        public decimal MaxAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public long AccountId { get; set; }

        public string AccountName { get; set; }
    }
}
