using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class InternalTransferRequest : BaseRequest
    {
        public decimal Amount { get; set; }
        public long? BeneficiaryId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public string Narration { get; set; } = null!;
        public string TransactionPin { get; set; } = null!;
    }
}
