using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class DepositModel
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string AccountName { get; set; }

        public string Email { get; set; }

        public string Reference { get; set; } = null!;

        public string? PayerAccountNumber { get; set; }

        public string? PayerAccountName { get; set; }

        public string? PayerBank { get; set; }

        public string Amount { get; set; }

        public string SettledAmount { get; set; }

        public decimal Fee { get; set; }

        public string Status { get; set; } = null!;

        public string Currency { get; set; } = null!;

        public string? Narration { get; set; }

        public string? Type { get; set; }

        public string? ResponseMessage { get; set; }

        public string? GatewayPostBack { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public string? PayerCountry { get; set; }
    }
}
