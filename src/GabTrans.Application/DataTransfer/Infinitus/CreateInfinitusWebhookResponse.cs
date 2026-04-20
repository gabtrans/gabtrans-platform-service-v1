using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateInfinitusWebhookResponse
    {
        public string id { get; set; }
        public string url { get; set; }
        public string secret { get; set; }
        public List<string> eventTypes { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
}
