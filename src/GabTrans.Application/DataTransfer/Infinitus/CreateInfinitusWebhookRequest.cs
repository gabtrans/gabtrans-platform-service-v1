using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.DataTransfer.Infinitus
{
    public class CreateInfinitusWebhookRequest
    {
        public string url { get; set; }
        public string secret { get; set; }
        public List<string> eventTypes { get; set; }
    }
}
