using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetRevenueRequest
    {
        public long? BillId { get; set; }
        public long? BillCategoryId { get; set; }
        public long? TransferTypeId { get; set; }
        public long? PaymentTypeId { get; set; }
        public long? TransactionStatusId { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool Complete { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CountryCode { get; set; }
    }
}
