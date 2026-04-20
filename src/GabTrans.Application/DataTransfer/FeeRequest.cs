using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class FeeRequest
    {
        public string TransactionType { get; set; }
        public long AccountId { get; set; }
        public decimal Rate { get; set; }
        public string Currency { get; set; }
        public decimal CappedAmount { get; set; }
    }
}
