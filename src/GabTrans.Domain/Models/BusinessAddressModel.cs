using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class BusinessAddressModel
    {
        public string BusinessLine1 { get; set; }
        public string BusinessLine2 { get; set; }
        public string BusinessCity { get; set; }
        public string BusinessState { get; set; }
        public string BusinessCountry { get; set; }
        public string BusinessPostalCode { get; set; }
        public string MailingLine1 { get; set; }
        public string MailingLine2 { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingCountry { get; set; }
        public string MailingPostalCode { get; set; }
    }
}
