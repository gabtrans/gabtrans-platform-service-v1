using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class BankTransferRequest
    {
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string AccountType { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string? PaymentMethod { get; set; }
        public string? CountryCode { get; set; }
        public string? Currency { get; set; }
        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string MetaData { get; set; }
    }
}
