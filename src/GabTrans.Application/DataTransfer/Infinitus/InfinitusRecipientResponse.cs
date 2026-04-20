using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class InfinitusRecipientResponse
    {
        public string id { get; set; }
        public string recipientAccountType { get; set; }
        public string paymentMethod { get; set; }
        public string? companyName { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public InfinitusRecipientAdditionalInfo additionalInfo { get; set; }
        public InfinitusRecipientAddress address { get; set; }
        public InfinitusRecipientBankDetails bankDetails { get; set; }
        public InfinitusRecipientIntermediaryBankAddress intermediaryBankAddress { get; set; }
        public string intermediaryBankName { get; set; }
        public string intermediaryRoutingCode { get; set; }
        public string internationalBankName { get; set; }
    }
}
