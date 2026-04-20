using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitoGlobalAccountResponse
    {
        public string id { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public List<InfinitoGlobalAccountTransferMethod> transferMethods { get; set; }
    }

    public class InfinitoGlobalAccountRoutingCode
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class InfinitoGlobalAccountTransferMethod
    {
        public string type { get; set; }
        public string accountNumber { get; set; }
        public string accountHolderName { get; set; }
        public List<InfinitoGlobalAccountRoutingCode> routingCodes { get; set; }
        public string referenceCode { get; set; }
        public string bankName { get; set; }
        public InfinitoGlobalAccountBankAddress bankAddress { get; set; }
    }

    public class InfinitoGlobalAccountBankAddress
    {
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }
}
