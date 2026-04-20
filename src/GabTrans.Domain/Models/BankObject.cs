using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BankObject
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string AccountType { get; set; }
        public string CurrencyCode { get; set; }
        public string SortCode { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
