using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer
{
    public class SendMailRequest
    {
        public string Receiver { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public List<MailAttachment>? Attachments { get; set; }
    }

    public class MailAttachment
    {
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }
        public string? MimeType { get; set; }
    }
}
