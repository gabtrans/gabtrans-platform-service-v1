using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class GetTransferSchemaRequest
    {
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public string BankCountryCode { get; set; }
        public string CountryCode { get; set; }
        public string TransferMethod { get; set; }
    }
}
