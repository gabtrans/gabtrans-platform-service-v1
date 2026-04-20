using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class TransferMeta
    {
        public string sender_name { get; set; }
        public string sender_address { get; set; }
        public string sender_country { get; set; }
        public string sender_currency { get; set; }
        public string sender_amount { get; set; }
    }
}
