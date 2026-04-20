using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class GetInfinitusRecipientSchemaRequest
    {
        public string recipientAccountType { get; set; }
        public string accountCurrency { get; set; }
        public string bankCountryCode { get; set; }
        public string countryCode { get; set; }
        public string transferMethod { get; set; }
    }
}
