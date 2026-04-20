using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Domain.Models
{
    public class EmailDetails
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public byte[]? Attachment { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
